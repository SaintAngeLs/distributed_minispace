namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentAlreadyInterestedInException : DomainException
    {
        public override string Code { get; } = "student_already_interested_in_event";
        public Guid StudentId { get; }
        public Guid EventId { get; }
        
        public StudentAlreadyInterestedInException(Guid studentId, Guid eventId)
            : base($"Student with id: {studentId} is already interested in event with id: {eventId}")
        {
            StudentId = studentId;
            EventId = eventId;
        }
    }
}