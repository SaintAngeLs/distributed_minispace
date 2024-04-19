using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadyRatedEventException : DomainException
    {
        public override string Code { get; } = "student_already_rated_event";
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentAlreadyRatedEventException(Guid eventId, Guid studentId) 
            : base($"Student with ID: '{studentId}' already rated event with ID: '{eventId}'.")
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}