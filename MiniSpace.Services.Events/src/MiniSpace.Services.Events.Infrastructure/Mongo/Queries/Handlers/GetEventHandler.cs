using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetEventHandler : IQueryHandler<GetEvent, EventDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;

        public GetEventHandler(IMongoRepository<EventDocument, Guid> eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public async Task<EventDto> HandleAsync(GetEvent query)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);
            
            return document?.AsDto();
        }
        
    }
}