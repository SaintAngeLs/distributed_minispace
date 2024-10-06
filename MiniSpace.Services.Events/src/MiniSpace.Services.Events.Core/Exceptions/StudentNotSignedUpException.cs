using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotSignedUpException : DomainException
    {
        public override string Code { get; } = "student_not_signed_up";
        public Guid UserId { get; }
        public Guid EventId { get; }

        public StudentNotSignedUpException(Guid userId, Guid eventId) 
            : base($"Student with ID: '{userId}' has not signed up to event with ID: '{eventId}'.")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}