using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedSearchEventsHandler : IQueryHandler<GetSearchEvents, MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;

        public GetPaginatedSearchEventsHandler(IEventRepository eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>> HandleAsync(GetSearchEvents query, CancellationToken cancellationToken)
        {
            // Convert string values to corresponding enum types
            Category? category = null;
            State? state = null;
            EventEngagementType? engagementType = null;

            if (!string.IsNullOrEmpty(query.Category) && Enum.TryParse(query.Category, out Category parsedCategory))
            {
                category = parsedCategory;
            }

            if (!string.IsNullOrEmpty(query.State) && Enum.TryParse(query.State, out State parsedState))
            {
                state = parsedState;
            }

            if (!string.IsNullOrEmpty(query.FriendsEngagementType) && Enum.TryParse(query.FriendsEngagementType, out EventEngagementType parsedEngagementType))
            {
                engagementType = parsedEngagementType;
            }

            // Use PageableDto for pagination and sorting
            var pageNumber = query.Pageable?.Page ?? 1;
            var pageSize = query.Pageable?.Size ?? 10;

            // Handle sorting
            var sortBy = query.Pageable?.Sort?.SortBy ?? Enumerable.Empty<string>();
            var sortDirection = query.Pageable?.Sort?.Direction ?? string.Empty;

            // Fetch the events based on the query parameters
            var (events, returnedPageNumber, returnedPageSize, totalPages, totalElements) = await _eventRepository.BrowseEventsAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                name: query.Name,
                organizer: query.Organizer,
                dateFrom: query.DateFrom ?? default(DateTime),
                dateTo: query.DateTo ?? default(DateTime),
                category: category,
                state: state,
                organizations: query.OrganizationId.HasValue ? new List<Guid> { query.OrganizationId.Value } : Enumerable.Empty<Guid>(), // Filter by OrganizationId
                friends: query.Friends ?? Enumerable.Empty<Guid>(),
                friendsEngagementType: engagementType,
                sortBy: sortBy,
                direction: sortDirection
            );

            // Map events to DTOs
            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            // Return the paginated result
            return new MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>(eventDtos, pageNumber, pageSize, totalElements);
        }
    }
}
