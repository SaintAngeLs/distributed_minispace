using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventRecommendationService
    {
        Task<IEnumerable<(EventDto Event, double Score)>> RankEventsByUserInterestAsync(Guid userId, IEnumerable<EventDto> events, IDictionary<string, double> userInterests);
    }
}
