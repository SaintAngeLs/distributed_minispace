using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetEventHandler : IQueryHandler<GetEvent, EventDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public GetEventHandler(IMongoRepository<EventDocument, Guid> eventRepository, 
                            IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task<EventDto> HandleAsync(GetEvent query, CancellationToken cancellationToken)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);
            if (document == null)
            {
                return null;
            }

            var identity = _appContext.Identity;

            await _messageBroker.PublishAsync(new EventViewed(query.EventId));

            return document.AsDto(identity.Id);
        }
    }

}
