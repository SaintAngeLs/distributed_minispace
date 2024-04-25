using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotSignedUpException : DomainException
    {
        public override string Code { get; } = "student_not_signed_up";
        public Guid StudentId { get; }
        public Guid EventId { get; }

        public StudentNotSignedUpException(Guid studentId, Guid eventId) 
            : base($"Student with ID: '{studentId}' has not signed up to event with ID: '{eventId}'.")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}