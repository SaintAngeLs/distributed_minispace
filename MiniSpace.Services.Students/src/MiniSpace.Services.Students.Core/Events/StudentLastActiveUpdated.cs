using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentLastActiveUpdated : IDomainEvent
    {
        public Guid UserId { get; }
        public DateTime LastActive { get; }

        public StudentLastActiveUpdated(User student)
        {
            UserId = student.Id;
            LastActive = student.LastActive ?? DateTime.UtcNow; 
        }
    }
}
