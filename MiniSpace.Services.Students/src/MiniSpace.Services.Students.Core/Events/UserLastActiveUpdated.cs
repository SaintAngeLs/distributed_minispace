using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserLastActiveUpdated : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime LastActive { get; }

        public UserLastActiveUpdated(User student)
        {
            UserId = student.Id;
            LastActive = student.LastActive ?? DateTime.UtcNow; 
        }
    }
}
