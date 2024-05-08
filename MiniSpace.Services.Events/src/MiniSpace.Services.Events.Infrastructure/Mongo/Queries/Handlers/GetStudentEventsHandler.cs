using System;
using System.Collections.Generic;
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
    public class GetStudentEventsHandler : IQueryHandler<GetStudentEvents, PagedResponse<IEnumerable<EventDto>>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IAppContext _appContext;
        
        public GetStudentEventsHandler(IEventRepository eventRepository, 
            IStudentsServiceClient studentsServiceClient, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _studentsServiceClient = studentsServiceClient;
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
            
            var studentEvents = await _studentsServiceClient.GetAsync(query.StudentId);
            var studentEventIds = studentEvents.InterestedInEvents.Union(studentEvents.SignedUpEvents).ToList();
            
            var result = await _eventRepository.BrowseEventsAsync(1, query.NumberOfResults,
                string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, null, null,
                Enumerable.Empty<Guid>(), Enumerable.Empty<string>(), "asc", studentEventIds);

            return new PagedResponse<IEnumerable<EventDto>>(result.Item1.Select(e => new EventDto(e, identity.Id)), 
                result.Item2, result.Item3, result.Item4, result.Item5);;
        }
    }
}