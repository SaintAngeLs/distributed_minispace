using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetEventRatingHandler : IQueryHandler<GetEventRating, EventRatingDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        
        public GetEventRatingHandler(IMongoRepository<EventDocument, Guid> eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public async Task<EventRatingDto> HandleAsync(GetEventRating query, CancellationToken cancellationToken)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);
            if (document is null)
            {
                return null;
            }

            var ratings = document.Ratings.Select(r => r.AsRatingDto()).ToList();
            var eventRatingDto = new EventRatingDto
            {
                EventId = document.Id,
                Ratings = ratings,
                TotalRatings = ratings.Count,
                AverageRating = ratings.Any() ? ratings.Average(r => r.Value) : 0
            };
            return eventRatingDto;
        }
    }
}
