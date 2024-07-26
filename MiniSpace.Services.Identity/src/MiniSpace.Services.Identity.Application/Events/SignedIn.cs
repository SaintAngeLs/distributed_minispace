using System;
using Convey.CQRS.Events;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.Events
{
    [Contract]
    public class SignedIn : IEvent
    {
        public Guid UserId { get; }
        public Role Role { get; }

        public SignedIn(Guid userId, Role role)
        {
            UserId = userId;
            Role = role;
        }
    }
}