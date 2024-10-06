using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadyInterestedInEventException : DomainException
    {
        public override string Code { get; } = "student_already_interested_in_event";
        public Guid EventId { get; }
        public Guid UserId { get; }

        public StudentAlreadyInterestedInEventException(Guid userId, Guid eventId) 
            : base($"Student with ID: {userId} is already interested in event with ID: {eventId}.")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}