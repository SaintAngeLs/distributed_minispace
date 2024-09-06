using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Core.Wrappers;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedEventsHandler : IQueryHandler<GetPaginatedEvents, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAppContext _appContext;

        public GetPaginatedEventsHandler(IEventRepository eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetPaginatedEvents query, CancellationToken cancellationToken)
        {
            var (events, pageNumber, pageSize, totalPages, totalElements) = await _eventRepository.BrowseEventsAsync(
                pageNumber: query.Page,
                pageSize: query.PageSize,
                name: string.Empty,
                organizer: string.Empty,
                dateFrom: default, 
                dateTo: default, 
                category: null, 
                state: null, 
                organizations: Enumerable.Empty<Guid>(),
                friends: Enumerable.Empty<Guid>(),
                friendsEngagementType: null, 
                sortBy: Enumerable.Empty<string>(),
                direction: string.Empty 
            );

            var studentId = _appContext.Identity.Id;
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            return new PagedResponse<EventDto>(eventDtos, pageNumber, pageSize, totalElements);
        }
    }
}
