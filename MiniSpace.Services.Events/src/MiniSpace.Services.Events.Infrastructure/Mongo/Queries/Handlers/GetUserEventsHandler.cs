using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Wrappers;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetUserEventsHandler : IQueryHandler<GetUserEvents, PagedResponse<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient; 
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;

        public GetUserEventsHandler(IEventRepository eventRepository, 
            IStudentsServiceClient studentsServiceClient, IEventValidator eventValidator, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventValidator = eventValidator;
            _appContext = appContext;
        }

        public async Task<PagedResponse<EventDto>> HandleAsync(GetUserEvents query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.UserId)
            {
                return new PagedResponse<EventDto>(Enumerable.Empty<EventDto>(), 1, query.NumberOfResults, 0);
            }

            int pageSize = query.NumberOfResults > 0 ? query.NumberOfResults : 10;

            var engagementType = _eventValidator.ParseEngagementType(query.EngagementType);

            var studentEvents = await _studentsServiceClient.GetAsync(query.UserId);
            var studentEventIds = engagementType switch
            {
                EventEngagementType.SignedUp => studentEvents.SignedUpEvents.ToList(),
                EventEngagementType.InterestedIn => studentEvents.InterestedInEvents.ToList(),
                _ => new List<Guid>()
            };

            var result = await _eventRepository.BrowseStudentEventsAsync(query.Page, 
                pageSize, studentEventIds, Enumerable.Empty<string>(), "asc");

            return new PagedResponse<EventDto>(
                result.events.Select(e => new EventDto(e, identity.Id)),
                result.pageNumber,
                result.pageSize,
                result.totalElements);
        }
    }
}
