using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Application.Services.Clients;
using MiniSpace.Services.Posts.Core.Wrappers;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Posts.Infrastructure.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostRepository _postRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IAppContext _appContext;

        public PostsService(IPostRepository postRepository, IStudentsServiceClient studentsServiceClient, 
            IAppContext appContext)
        {
            _postRepository = postRepository;
            _studentsServiceClient = studentsServiceClient;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<PostDto>>> BrowsePostsAsync(SearchPosts command)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != command.StudentId)
            {
                throw new UnauthorizedPostSearchException(command.StudentId, identity.Id);
            }

            var studentEvents = await _studentsServiceClient.GetAsync(command.StudentId);
            if (studentEvents is null)
            {
                throw new InvalidStudentServiceClientResponseException(command.StudentId);
            }

            var eventsIds = studentEvents.InterestedInEvents.Union(studentEvents.SignedUpEvents).ToList();

            var pageNumber = command.Pageable.Page < 1 ? 1 : command.Pageable.Page;
            var pageSize = command.Pageable.Size > 10 ? 10 : command.Pageable.Size;

            var result = await _postRepository.BrowseCommentsAsync(
                pageNumber, pageSize, eventsIds, command.Pageable.Sort.SortBy, command.Pageable.Sort.Direction);

            var pagedEvents = new PagedResponse<IEnumerable<PostDto>>(
                result.posts.Select(p => new PostDto(p)),
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);

            return pagedEvents;
        }
    }
}