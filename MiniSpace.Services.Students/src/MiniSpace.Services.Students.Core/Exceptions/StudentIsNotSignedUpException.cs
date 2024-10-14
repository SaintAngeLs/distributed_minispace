namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentIsNotSignedUpException : DomainException
    {
        public override string Code { get; } = "student_is_not_signed_up_to_event";
        public Guid UserId { get; }
        public Guid EventId { get; }
        
        public StudentIsNotSignedUpException(Guid userId, Guid eventId)
            : base($"Student with id: {userId} is not signed up to event with id: {eventId}")
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}