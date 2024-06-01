namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentAlreadySignedUpException : DomainException
    {
        public override string Code { get; } = "student_already_signed_up";
        public Guid StudentId { get; }
        public Guid EventId { get; }
        
        public StudentAlreadySignedUpException(Guid studentId, Guid eventId)
            : base($"Student with id: {studentId} is already signed up for event with id: {eventId}")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}