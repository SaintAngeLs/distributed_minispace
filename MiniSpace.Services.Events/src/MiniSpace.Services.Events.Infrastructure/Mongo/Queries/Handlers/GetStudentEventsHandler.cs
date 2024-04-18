using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentEventsHandler : IQueryHandler<GetStudentEvents, IEnumerable<EventDto>>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        private readonly IAppContext _appContext;
        
        public GetStudentEventsHandler(IMongoRepository<EventDocument, Guid> eventRepository, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<EventDto>> HandleAsync(GetStudentEvents query)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != query.StudentId)
            {
                return Enumerable.Empty<EventDto>();
            }
            
            // TODO: this query should be optimized
            var documents = await _eventRepository.FindAsync(
                x => x.InterestedStudents.Any(i => i.StudentId == query.StudentId) ||
                     x.SignedUpStudents.Any(i => i.StudentId == query.StudentId));
            
            var newestDocuments = documents.OrderByDescending(x => x.PublishDate).Take(5);

            return newestDocuments.Select(x => x.AsDto());
        }
    }
}