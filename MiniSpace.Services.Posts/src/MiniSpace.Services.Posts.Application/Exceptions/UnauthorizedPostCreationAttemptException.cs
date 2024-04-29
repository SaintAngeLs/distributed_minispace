namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class UnauthorizedPostCreationAttemptException : AppException
    {
        public override string Code => "unauthorized_post_creation_attempt";
        public Guid OrganizerId { get; }
        public Guid EventId { get; }
        
        public UnauthorizedPostCreationAttemptException(Guid organizerId, Guid eventId)
            : base("Unauthorized post creation attempt. Organizer with ID: {OrganizerId} tried to create a post for event with ID: {EventId}.")
        {
            OrganizerId = organizerId;
            EventId = eventId;
        }
    }
}