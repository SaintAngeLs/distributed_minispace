using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.DTO;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserPostViewsHandler : IQueryHandler<GetUserPostViews, PagedResponse<ViewDto>>
    {
        private readonly IPostsUserViewsRepository _postsUserViewsRepository;

        public GetUserPostViewsHandler(IPostsUserViewsRepository postsUserViewsRepository)
        {
            _postsUserViewsRepository = postsUserViewsRepository;
        }

        public async Task<PagedResponse<ViewDto>> HandleAsync(GetUserPostViews query, CancellationToken cancellationToken)
        {
            // Retrieve the user's post views
            var userViews = await _postsUserViewsRepository.GetAsync(query.UserId);

            if (userViews == null || !userViews.Views.Any())
            {
                return new PagedResponse<ViewDto>(Enumerable.Empty<ViewDto>(), query.PageNumber, query.PageSize, 0);
            }

            // Paginate the views
            var pagedViews = userViews.Views
                .OrderByDescending(v => v.Date)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(v => new ViewDto { PostId = v.PostId, Date = v.Date })
                .ToList();

            return new PagedResponse<ViewDto>(pagedViews, query.PageNumber, query.PageSize, userViews.Views.Count());
        }
    }
}
