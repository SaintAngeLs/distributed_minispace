using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentOnlineStatusChanged : IDomainEvent
    {
        public Guid StudentId { get; }
        public bool IsOnline { get; }
        public string DeviceType { get; }
        public DateTime? LastActive { get; }

        public StudentOnlineStatusChanged(Student student)
        {
            StudentId = student.Id;
            IsOnline = student.IsOnline;
            DeviceType = student.DeviceType;
            LastActive = student.LastActive;
        }
    }
}
