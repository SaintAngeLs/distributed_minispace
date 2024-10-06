using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Infrastructure.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppContext _appContext;

        public PostsService(
            IPostRepository postRepository,
            IUserPostRepository userPostRepository,
            IOrganizationPostRepository organizationPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IDateTimeProvider dateTimeProvider,
            IAppContext appContext)
        {
            _postRepository = postRepository;
            _userPostRepository = userPostRepository;
            _organizationPostRepository = organizationPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _dateTimeProvider = dateTimeProvider;
            _appContext = appContext;
        }

        public async Task<PagedResponse<PostDto>> BrowsePostsAsync(BrowseRequest request)
        {
            var identity = _appContext.Identity;

            if (request.UserId.HasValue && identity.IsAuthenticated && identity.Id != request.UserId.Value)
            {
                throw new UnauthorizedPostSearchException(request.UserId.Value, identity.Id);
            }

            PagedResponse<Post> pagedResponse = null;

            if (request.OrganizationId.HasValue && request.EventId.HasValue)
            {
                // Browsing posts by organization event
                pagedResponse = await _organizationEventPostRepository.BrowseOrganizationEventPostsAsync(request);
            }
            else if (request.OrganizationId.HasValue)
            {
                // Browsing posts by organization
                pagedResponse = await _organizationPostRepository.BrowseOrganizationPostsAsync(request);
            }
            else if (request.UserId.HasValue && request.EventId.HasValue)
            {
                // Browsing posts by user event
                pagedResponse = await _userEventPostRepository.BrowseUserEventPostsAsync(request);
            }
            else if (request.UserId.HasValue)
            {
                // Browsing posts by user
                pagedResponse = await _userPostRepository.BrowseUserPostsAsync(request);
            }
            else
            {
                // Return posts from all repositories
                var orgEventPosts = await _organizationEventPostRepository.BrowsePostsAsync(request);
                var orgPosts = await _organizationPostRepository.BrowsePostsAsync(request);
                var userEventPosts = await _userEventPostRepository.BrowsePostsAsync(request);
                var userPosts = await _userPostRepository.BrowsePostsAsync(request);

                var allPosts = orgEventPosts.Items
                    .Concat(orgPosts.Items)
                    .Concat(userEventPosts.Items)
                    .Concat(userPosts.Items)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                pagedResponse = new PagedResponse<Post>(
                    allPosts,
                    request.PageNumber,
                    request.PageSize,
                    orgEventPosts.TotalItems + orgPosts.TotalItems + userEventPosts.TotalItems + userPosts.TotalItems
                );
            }

            return new PagedResponse<PostDto>(
                pagedResponse.Items.Select(p => new PostDto(p)),
                pagedResponse.Page,
                pagedResponse.PageSize,
                pagedResponse.TotalItems
            );
        }

        public async Task<Post> CreatePostAsync(CreatePost command)
        {
            var identity = _appContext.Identity;

            // Check if the user is authorized to create posts on the specified page
            if (identity.IsAuthenticated && identity.Id != command.UserId && identity.Id != command.OrganizationId && identity.Id != command.PageOwnerId)
            {
                throw new UnauthorizedPostCreationAttemptException(identity.Id, command.EventId ?? Guid.Empty);
            }

            if (command.PostId == Guid.Empty || await _postRepository.ExistsAsync(command.PostId))
            {
                throw new InvalidPostIdException(command.PostId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var newState))
            {
                throw new InvalidPostStateException(command.State);
            }

            if (!Enum.TryParse<VisibilityStatus>(command.Visibility, true, out var visibilityStatus))
            {
                throw new InvalidVisibilityStatusException(command.Visibility);
            }

            var mediaFiles = command.MediaFiles.ToList();
            if (mediaFiles.Count > 12)
            {
                throw new InvalidNumberOfPostMediaFilesException(command.PostId, mediaFiles.Count);
            }

            switch (newState)
            {
                case State.Reported:
                    throw new NotAllowedPostStateException(command.PostId, newState);
                case State.ToBePublished when command.PublishDate is null:
                    throw new PublishDateNullException(command.PostId, newState);
            }

            Post post;

            // Check where the post is being created based on the context and page ownership
            if (command.Context == PostContext.UserPage)
            {
                // If creating a post on another user's page, use the PageOwnerId
                post = Post.CreateForUser(command.PostId, command.PageOwnerId ?? command.UserId.Value, command.TextContent, command.MediaFiles,
                    _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                await _userPostRepository.AddAsync(post);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                // If creating a post on an organization page, use the PageOwnerId for the organization
                post = Post.CreateForOrganization(command.PostId, command.PageOwnerId ?? command.OrganizationId.Value, command.UserId, command.TextContent, command.MediaFiles,
                    _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                await _organizationPostRepository.AddAsync(post);
            }
            else if (command.Context == PostContext.EventPage)
            {
                // Handle event posts
                if (command.UserId.HasValue)
                {
                    post = Post.CreateForEvent(command.PostId, command.EventId.Value, command.UserId, command.OrganizationId, command.TextContent,
                        command.MediaFiles, _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                    await _userEventPostRepository.AddAsync(post);
                }
                else
                {
                    post = Post.CreateForEvent(command.PostId, command.EventId.Value, null, command.OrganizationId, command.TextContent,
                        command.MediaFiles, _dateTimeProvider.Now, newState, command.PublishDate, visibilityStatus);
                    await _organizationEventPostRepository.AddAsync(post);
                }
            }
            else
            {
                throw new InvalidPostContextException(command.Context.ToString());
            }

            return post;
        }

        public async Task<Post> UpdatePostAsync(UpdatePost command)
        {
            Post post = null;

            // Fetch the correct post based on context (User, Organization, Event)
            if (command.Context == PostContext.UserPage)
            {
                post = await _userPostRepository.GetAsync(command.PostId);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                post = await _organizationPostRepository.GetAsync(command.PostId);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    post = await _userEventPostRepository.GetAsync(command.PostId);
                }
                else
                {
                    post = await _organizationEventPostRepository.GetAsync(command.PostId);
                }
            }

            if (post is null)
            {
                throw new PostNotFoundException(command.PostId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != post.UserId && identity.Id != post.OrganizationId && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }

            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }

            // Parse and validate the state and visibility
            State? newState = null;
            if (!string.IsNullOrWhiteSpace(command.State))
            {
                if (!Enum.TryParse<State>(command.State, true, out var parsedState))
                {
                    throw new InvalidPostStateException(command.State);
                }
                newState = parsedState;
            }

            VisibilityStatus? newVisibility = null;
            if (!string.IsNullOrWhiteSpace(command.Visibility))
            {
                if (!Enum.TryParse<VisibilityStatus>(command.Visibility, true, out var parsedVisibility))
                {
                    throw new InvalidVisibilityStatusException(command.Visibility);
                }
                newVisibility = parsedVisibility;
            }

            // Check and validate media files
            var mediaFiles = command.MediaFiles.ToList();
            if (mediaFiles.Count > 12)
            {
                throw new InvalidNumberOfPostMediaFilesException(post.Id, mediaFiles.Count);
            }

            // Update the post's text content, media, and state
            post.Update(command.TextContent, mediaFiles, _dateTimeProvider.Now);

            if (newState.HasValue)
            {
                post.ChangeState(newState.Value, command.PublishDate, _dateTimeProvider.Now);
            }

            if (newVisibility.HasValue)
            {
                post.SetVisibility(newVisibility.Value, _dateTimeProvider.Now);
            }

            // Update the post in the correct repository
            if (command.Context == PostContext.UserPage)
            {
                await _userPostRepository.UpdateAsync(post);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                await _organizationPostRepository.UpdateAsync(post);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    await _userEventPostRepository.UpdateAsync(post);
                }
                else
                {
                    await _organizationEventPostRepository.UpdateAsync(post);
                }
            }

            return post;
        }

        public async Task<Post> RepostPostAsync(RepostCommand command)
        {
            Post originalPost = null;

            // Fetch the original post based on context
            if (command.Context == PostContext.UserPage)
            {
                originalPost = await _userPostRepository.GetAsync(command.OriginalPostId);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                originalPost = await _organizationPostRepository.GetAsync(command.OriginalPostId);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    originalPost = await _userEventPostRepository.GetAsync(command.OriginalPostId);
                }
                else
                {
                    originalPost = await _organizationEventPostRepository.GetAsync(command.OriginalPostId);
                }
            }

            if (originalPost is null)
            {
                throw new PostNotFoundException(command.OriginalPostId);
            }

            // Create the repost based on the original post
            var repost = Post.CreateRepost(
                command.RepostedPostId,
                command.PageOwnerId ?? command.UserId ?? Guid.Empty, // Reposting by user or page owner
                originalPost,
                _dateTimeProvider.Now,
                State.Published
            );

            // Add repost to the correct repository based on context
            if (command.Context == PostContext.UserPage)
            {
                await _userPostRepository.AddAsync(repost);
            }
            else if (command.Context == PostContext.OrganizationPage)
            {
                await _organizationPostRepository.AddAsync(repost);
            }
            else if (command.Context == PostContext.EventPage)
            {
                if (command.UserId.HasValue)
                {
                    await _userEventPostRepository.AddAsync(repost);
                }
                else
                {
                    await _organizationEventPostRepository.AddAsync(repost);
                }
            }

            return repost;
        }

        public async Task DeletePostAsync(DeletePost command)
        {
            Post post = null;

            // Determine where to delete the post based on the context (user page, org page, or event page)
            switch (command.Context.ToLowerInvariant())
            {
                case "userpage":
                    post = await _userPostRepository.GetAsync(command.PostId);
                    break;
                case "organizationpage":
                    post = await _organizationPostRepository.GetAsync(command.PostId);
                    break;
                case "eventpage" when command.UserId.HasValue:
                    post = (await _userEventPostRepository.GetByUserEventIdAsync(command.UserId.Value, command.EventId.Value))
                        .FirstOrDefault(p => p.Id == command.PostId);
                    break;
                case "eventpage" when command.OrganizationId.HasValue:
                    post = (await _organizationEventPostRepository.GetByOrganizationEventIdAsync(command.OrganizationId.Value, command.EventId.Value))
                        .FirstOrDefault(p => p.Id == command.PostId);
                    break;
                default:
                    throw new InvalidPostContextException(command.Context);
            }

            if (post == null)
            {
                throw new PostNotFoundException(command.PostId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != (post.UserId ?? post.OrganizationId) && !identity.IsAdmin)
            {
                throw new UnauthorizedPostAccessException(command.PostId, identity.Id);
            }

            if (!identity.IsAdmin && post.State == State.Reported)
            {
                throw new UnauthorizedPostOperationException(command.PostId, identity.Id);
            }

            // Perform the deletion
            switch (command.Context.ToLowerInvariant())
            {
                case "userpage":
                    await _userPostRepository.DeleteAsync(command.PostId);
                    break;
                case "organizationpage":
                    await _organizationPostRepository.DeleteAsync(command.PostId);
                    break;
                case "eventpage" when command.UserId.HasValue:
                    await _userEventPostRepository.DeleteAsync(command.PostId);
                    break;
                case "eventpage" when command.OrganizationId.HasValue:
                    await _organizationEventPostRepository.DeleteAsync(command.PostId);
                    break;
                default:
                    throw new InvalidPostContextException(command.Context);
            }
        }
    }
}
