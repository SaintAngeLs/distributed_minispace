using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadyInterestedInEventException : DomainException
    {
        public override string Code { get; } = "student_already_interested_in_event";
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentAlreadyInterestedInEventException(Guid studentId, Guid eventId) 
            : base($"Student with ID: {studentId} is already interested in event with ID: {eventId}.")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}