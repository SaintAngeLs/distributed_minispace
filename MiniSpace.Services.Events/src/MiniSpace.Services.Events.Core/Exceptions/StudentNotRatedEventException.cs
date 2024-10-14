using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotRatedEventException : DomainException
    {
        public override string Code { get; } = "student_not_rated_event";
        public Guid EventId { get; }
        public Guid UserId { get; }

        public StudentNotRatedEventException(Guid eventId, Guid userId) 
            : base($"Student with ID: '{userId}' has not rated event with ID: '{eventId}'.")
        {
            EventId = eventId;
            UserId = userId;
        }
    }
}