using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentNotInterestedInEventException : DomainException
    {
        public override string Code { get; } = "student_is_not_interested_in_event";
        public Guid UserId { get; }
        public Guid EventId { get; }

        public StudentNotInterestedInEventException(Guid userId, Guid eventId) 
            : base($"Student with ID: '{userId}' is not interested in event with ID: '{eventId}'.")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}