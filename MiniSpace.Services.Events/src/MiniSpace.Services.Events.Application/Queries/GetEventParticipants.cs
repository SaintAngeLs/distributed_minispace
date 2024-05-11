using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetEventParticipants : IQuery<EventParticipantsDto>
    {
        public Guid EventId { get; set; }
    }
}