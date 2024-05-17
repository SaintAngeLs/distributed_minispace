using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetEventRatingHandler : IQueryHandler<GetEventRating, EventRatingDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        private readonly IAppContext _appContext;
        
        public GetEventRatingHandler(IMongoRepository<EventDocument, Guid> eventRepository,
            IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _appContext = appContext;
        }
        
        public async Task<EventRatingDto> HandleAsync(GetEventRating query, CancellationToken cancellationToken)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);
            if(document is null)
            {
                return null;
            }
            var identity = _appContext.Identity;
            if(identity.IsAuthenticated && identity.Id != document.Organizer.Id && !identity.IsAdmin)
            {
                return null;
            }
            
            return document.AsRatingDto();
        }

    }
}