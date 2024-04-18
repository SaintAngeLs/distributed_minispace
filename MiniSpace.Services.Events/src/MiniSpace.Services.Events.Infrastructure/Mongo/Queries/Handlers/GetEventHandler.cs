using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetEventHandler : IQueryHandler<GetEvent, EventDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        private readonly IMessageBroker _messageBroker;

        public GetEventHandler(IMongoRepository<EventDocument, Guid> eventRepository, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task<EventDto> HandleAsync(GetEvent query, CancellationToken cancellationToken)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);

            await _messageBroker.PublishAsync(new EventViewed(query.EventId));
            return document?.AsDto();
        }
        
    }
}