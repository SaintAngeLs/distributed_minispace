namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentIsNotInterestedException : DomainException
    {
        public override string Code { get; } = "student_is_not_interested_in_event";
        public Guid StudentId { get; }
        public Guid EventId { get; }
        
        public StudentIsNotInterestedException(Guid studentId, Guid eventId)
            : base($"Student with id: {studentId} is not interested in event with id: {eventId}")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}