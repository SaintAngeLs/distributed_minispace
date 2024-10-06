using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class OrganizerRightsGranted : IEvent
    {
        public Guid UserId { get; }
        public OrganizerRightsGranted(Guid userId)
        {
            UserId = userId;
        }
    }
}