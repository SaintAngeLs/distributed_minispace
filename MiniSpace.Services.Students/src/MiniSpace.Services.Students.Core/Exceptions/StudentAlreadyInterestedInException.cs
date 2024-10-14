namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentAlreadyInterestedInException : DomainException
    {
        public override string Code { get; } = "student_already_interested_in_event";
        public Guid UserId { get; }
        public Guid EventId { get; }
        
        public StudentAlreadyInterestedInException(Guid userId, Guid eventId)
            : base($"Student with id: {userId} is already interested in event with id: {eventId}")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}