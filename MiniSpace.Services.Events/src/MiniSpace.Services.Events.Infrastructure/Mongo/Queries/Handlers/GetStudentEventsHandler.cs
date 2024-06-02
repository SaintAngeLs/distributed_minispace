using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetStudentEventsHandler : IQueryHandler<GetStudentEvents, PagedResponse<IEnumerable<EventDto>>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;
        
        public GetStudentEventsHandler(IEventRepository eventRepository, 
            IStudentsServiceClient studentsServiceClient, IEventValidator eventValidator, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
            _eventValidator = eventValidator;
            _appContext = appContext;
        }

        public async Task<PagedResponse<IEnumerable<EventDto>>> HandleAsync(GetStudentEvents query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.StudentId)
            {
                return new PagedResponse<IEnumerable<EventDto>>(Enumerable.Empty<EventDto>(),
                    1, query.NumberOfResults, 0, 0);
            }
            
            var engagementType = _eventValidator.ParseEngagementType(query.EngagementType);
            
            var studentEvents = await _studentsServiceClient.GetAsync(query.StudentId);
            var studentEventIds = engagementType switch
            {
                EventEngagementType.SignedUp => studentEvents.SignedUpEvents.ToList(),
                EventEngagementType.InterestedIn => studentEvents.InterestedInEvents.ToList(),
                _ => []
            };
            
            var result = await _eventRepository.BrowseStudentEventsAsync(query.Page, 
                query.NumberOfResults, studentEventIds, Enumerable.Empty<string>(), "asc");

            return new PagedResponse<IEnumerable<EventDto>>(result.events.Select(e => new EventDto(e, identity.Id)), 
                result.pageNumber, result.pageSize, result.totalPages, result.totalElements);
        }
    }
}