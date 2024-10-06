using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetEventParticipants : IQuery<EventParticipantsDto>
    {
        public Guid EventId { get; set; }
    }
}