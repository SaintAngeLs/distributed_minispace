using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotInterestedInEventException : DomainException
    {
        public override string Code { get; } = "student_is_not_interested_in_event";
        public Guid StudentId { get; }
        public Guid EventId { get; }

        public StudentNotInterestedInEventException(Guid studentId, Guid eventId) 
            : base($"Student with ID: '{studentId}' is not interested in event with ID: '{eventId}'.")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}