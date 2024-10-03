using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Wrappers;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedOrganizerEventsHandler : IQueryHandler<GetPaginatedOrganizerEvents, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;

        public GetPaginatedOrganizerEventsHandler(IEventRepository eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetPaginatedOrganizerEvents query, CancellationToken cancellationToken)
        {
            var (events, pageNumber, pageSize, totalPages, totalElements) = await _eventRepository.BrowseOrganizerEventsAsync(
                pageNumber: query.Page,
                pageSize: query.PageSize,
                name: string.Empty,
                organizerId: query.OrganizerId,
                dateFrom: default,
                dateTo: default,
                sortBy: Enumerable.Empty<string>(),
                direction: string.Empty,
                state: null
            );

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            return new PagedResponse<EventDto>(eventDtos, pageNumber, pageSize, totalElements);
        }
    }
}
