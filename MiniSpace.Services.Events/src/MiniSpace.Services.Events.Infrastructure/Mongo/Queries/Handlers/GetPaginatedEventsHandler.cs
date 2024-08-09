using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedEventsHandler : IQueryHandler<GetPaginatedEvents, MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;

        public GetPaginatedEventsHandler(IEventRepository eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>> HandleAsync(GetPaginatedEvents query, CancellationToken cancellationToken)
        {
            // Fetch the paginated events from the repository
            var (events, pageNumber, pageSize, totalPages, totalElements) = await _eventRepository.BrowseEventsAsync(
                pageNumber: query.Page,
                pageSize: query.PageSize,
                name: string.Empty, // Assuming no filtering by name
                organizer: string.Empty, // Assuming no filtering by organizer
                dateFrom: default, // Assuming no date filter
                dateTo: default, // Assuming no date filter
                category: null, // Assuming no category filter
                state: null, // Assuming no state filter
                organizations: Enumerable.Empty<Guid>(), // Assuming no organization filter
                friends: Enumerable.Empty<Guid>(), // Assuming no friends filter
                friendsEngagementType: null, // Assuming no engagement type filter
                sortBy: Enumerable.Empty<string>(), // Assuming no sorting
                direction: string.Empty // Assuming no sorting direction
            );

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            // Return a paged result with the fetched data
            return new MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>(eventDtos, pageNumber, pageSize, totalElements);
        }
    }
}
