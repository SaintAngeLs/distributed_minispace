using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries.Handlers
{
    public class GetPaginatedUserViewsHandler : IQueryHandler<GetPaginatedUserViews, PagedResponse<ViewDto>>
    {
        private readonly IEventsUserViewsRepository _eventsUserViewsRepository;

        public GetPaginatedUserViewsHandler(IEventsUserViewsRepository eventsUserViewsRepository)
        {
            _eventsUserViewsRepository = eventsUserViewsRepository;
        }

        public async Task<PagedResponse<ViewDto>> HandleAsync(GetPaginatedUserViews query, CancellationToken cancellationToken)
        {
            var userViews = await _eventsUserViewsRepository.GetAsync(query.UserId);

            if (userViews == null)
            {
                return new PagedResponse<ViewDto>(Enumerable.Empty<ViewDto>(), query.Page, query.PageSize, 0);
            }

            var totalItems = userViews.Views.Count();
            var views = userViews.Views
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(view => new ViewDto
                {
                    EventId = view.EventId,
                    Date = view.Date
                })
                .ToList();

            return new PagedResponse<ViewDto>(views, query.Page, query.PageSize, totalItems);
        }
    }
}
