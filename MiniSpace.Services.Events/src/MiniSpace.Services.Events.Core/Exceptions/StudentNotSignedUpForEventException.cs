using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotSignedUpForEventException : DomainException
    {
        public override string Code { get; } = "student_not_signed_for_event";
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentNotSignedUpForEventException(Guid eventId, Guid studentId) 
            : base($"Student with ID: '{studentId}' is not signed for event with ID: '{eventId}'.")
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}