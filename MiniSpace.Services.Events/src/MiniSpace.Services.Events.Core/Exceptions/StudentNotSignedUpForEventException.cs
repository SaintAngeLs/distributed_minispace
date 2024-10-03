using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotSignedUpForEventException : DomainException
    {
        public override string Code { get; } = "student_not_signed_for_event";
        public Guid EventId { get; }
        public Guid UserId { get; }

        public StudentNotSignedUpForEventException(Guid eventId, Guid userId) 
            : base($"Student with ID: '{userId}' is not signed for event with ID: '{eventId}'.")
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}