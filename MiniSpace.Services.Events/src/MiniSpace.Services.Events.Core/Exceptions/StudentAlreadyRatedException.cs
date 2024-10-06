using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadyRatedException : DomainException
    {
        public override string Code { get; } = "student_already_rated";
        public Guid UserId { get; }
        public Guid EventId { get; }

        public StudentAlreadyRatedException(Guid userId, Guid eventId) : base(
            $"Student with ID: {userId} has already rated event with ID: {eventId}.")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}