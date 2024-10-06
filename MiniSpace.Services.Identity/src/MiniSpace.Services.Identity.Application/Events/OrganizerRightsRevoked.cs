using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class OrganizerRightsRevoked : IEvent
    {
        public Guid UserId { get; }
        public OrganizerRightsRevoked(Guid userId)
        {
            UserId = userId;
        }
    }
}