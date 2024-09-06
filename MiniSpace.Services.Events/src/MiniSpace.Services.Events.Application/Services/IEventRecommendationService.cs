using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventRecommendationService
    {
        IEnumerable<EventDto> RankEventsByUserInterest(Guid userId, IEnumerable<EventDto> events, IEnumerable<string> userInterests);
    }
}
