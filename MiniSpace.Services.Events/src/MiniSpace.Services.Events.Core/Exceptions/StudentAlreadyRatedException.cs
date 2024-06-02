using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class StudentAlreadyRatedException : DomainException
    {
        public override string Code { get; } = "student_already_rated";
        public Guid StudentId { get; }
        public Guid EventId { get; }

        public StudentAlreadyRatedException(Guid studentId, Guid eventId) : base(
            $"Student with ID: {studentId} has already rated event with ID: {eventId}.")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}