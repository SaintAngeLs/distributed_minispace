namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class UserIsNotInterestedException : DomainException
    {
        public override string Code { get; } = "student_is_not_interested_in_event";
        public Guid UserId { get; }
        public Guid EventId { get; }
        
        public UserIsNotInterestedException(Guid userId, Guid eventId)
            : base($"Student with id: {userId} is not interested in event with id: {eventId}")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}