using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetEventRating : IQuery<EventRatingDto>
    {
        public Guid EventId { get; set; }
    }
}