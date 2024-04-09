using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetEvent : IQuery<EventDto>
    {
        public Guid EventId { get; set; }
    }
}