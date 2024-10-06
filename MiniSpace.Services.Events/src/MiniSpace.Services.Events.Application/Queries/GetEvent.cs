using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetEvent : IQuery<EventDto>
    {
        public Guid EventId { get; set; }
    }
}