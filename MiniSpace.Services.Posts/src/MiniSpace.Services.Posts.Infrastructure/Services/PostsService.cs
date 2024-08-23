using MiniSpace.Services.Posts.Application;
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
        private readonly IOrganizationPostRepository _organizationPostRepository;
        private readonly IOrganizationEventPostRepository _organizationEventPostRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IUserEventPostRepository _userEventPostRepository;
        private readonly IPostRepository _postRepository;
        private readonly IAppContext _appContext;

        public PostsService(
            IOrganizationPostRepository organizationPostRepository,
            IOrganizationEventPostRepository organizationEventPostRepository,
            IUserPostRepository userPostRepository,
            IUserEventPostRepository userEventPostRepository,
            IPostRepository postRepository,
            IAppContext appContext)
        {
            _organizationPostRepository = organizationPostRepository;
            _organizationEventPostRepository = organizationEventPostRepository;
            _userPostRepository = userPostRepository;
            _userEventPostRepository = userEventPostRepository;
            _postRepository = postRepository;
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
    }
}
