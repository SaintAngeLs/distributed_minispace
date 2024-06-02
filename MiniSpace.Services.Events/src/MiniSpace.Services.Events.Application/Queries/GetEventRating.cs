using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetEventRating : IQuery<EventRatingDto>
    {
        public Guid EventId { get; set; }
    }
}