namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class UserAlreadySignedUpException : DomainException
    {
        public override string Code { get; } = "student_already_signed_up";
        public Guid UserId { get; }
        public Guid EventId { get; }
        
        public UserAlreadySignedUpException(Guid userId, Guid eventId)
            : base($"Student with id: {userId} is already signed up for event with id: {eventId}")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}