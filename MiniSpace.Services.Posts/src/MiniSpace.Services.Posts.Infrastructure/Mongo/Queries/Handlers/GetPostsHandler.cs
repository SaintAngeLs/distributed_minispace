using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPostsHandler : IQueryHandler<GetPosts, PagedResponse<PostDto>>
    {
        private readonly IPostsService _postsService;

        public GetPostsHandler(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public async Task<PagedResponse<PostDto>> HandleAsync(GetPosts query, CancellationToken cancellationToken)
        {
            var request = new BrowseRequest
            {
                UserId = query.UserId,
                OrganizationId = query.OrganizationId,
                EventId = query.EventId,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortBy = query.SortBy,
                Direction = query.Direction
            };

            var result = await _postsService.BrowsePostsAsync(request);

            return result;
        }
    }
}
