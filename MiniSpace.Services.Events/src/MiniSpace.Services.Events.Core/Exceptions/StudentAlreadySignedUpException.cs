using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadySignedUpException : DomainException
    {
        public override string Code { get; } = "student_already_signed_up";
        public Guid StudentId { get; }
        public Guid EventId { get; }

        public StudentAlreadySignedUpException(Guid studentId, Guid eventId) : base(
            $"Student with ID: {studentId} already signed up to event with ID: {eventId}.")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}