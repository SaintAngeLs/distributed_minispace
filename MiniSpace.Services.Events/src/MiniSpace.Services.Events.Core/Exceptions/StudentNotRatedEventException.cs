using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotRatedEventException : DomainException
    {
        public override string Code { get; } = "student_not_rated_event";
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentNotRatedEventException(Guid eventId, Guid studentId) 
            : base($"Student with ID: '{studentId}' has not rated event with ID: '{eventId}'.")
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}