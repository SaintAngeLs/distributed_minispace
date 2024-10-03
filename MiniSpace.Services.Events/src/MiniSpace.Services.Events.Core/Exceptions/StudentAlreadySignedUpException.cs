using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadySignedUpException : DomainException
    {
        public override string Code { get; } = "student_already_signed_up";
        public Guid UserId { get; }
        public Guid EventId { get; }

        public StudentAlreadySignedUpException(Guid userId, Guid eventId) : base(
            $"Student with ID: {userId} already signed up to event with ID: {eventId}.")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}