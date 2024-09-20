using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Wrappers;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedSearchEventsHandler : IQueryHandler<GetSearchEvents, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;

        public GetPaginatedSearchEventsHandler(IEventRepository eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetSearchEvents query, CancellationToken cancellationToken)
        {
            var jsonOptionsx = new JsonSerializerOptions { WriteIndented = true };
            var queryJson = JsonSerializer.Serialize(query, jsonOptionsx);
            Console.WriteLine("Query Object: ");
            Console.WriteLine(queryJson);

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

            var pageNumber = query.Pageable?.Page ?? 1;
            var pageSize = query.Pageable?.Size ?? 10;

            var sortBy = query.Pageable?.Sort?.SortBy ?? Enumerable.Empty<string>();
            var sortDirection = query.Pageable?.Sort?.Direction ?? string.Empty;

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

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            var pagedResult = new  PagedResponse<EventDto>(eventDtos, pageNumber, pageSize, totalElements);

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonResult = JsonSerializer.Serialize(pagedResult, jsonOptions);
            Console.WriteLine("Search Results: ");
            Console.WriteLine(jsonResult);

            return pagedResult;
        }
    }
}
