namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentIsNotSignedUpException : DomainException
    {
        public override string Code { get; } = "student_is_not_signed_up_to_event";
        public Guid StudentId { get; }
        public Guid EventId { get; }
        
        public StudentIsNotSignedUpException(Guid studentId, Guid eventId)
            : base($"Student with id: {studentId} is not signed up to event with id: {eventId}")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}