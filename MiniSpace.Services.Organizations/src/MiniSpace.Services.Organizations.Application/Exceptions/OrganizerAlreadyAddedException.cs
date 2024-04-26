namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class OrganizerAlreadyAddedException : AppException
    {
        public override string Code { get; } = "organizer_already_added";
        public Guid OrganizerId { get; }

        public OrganizerAlreadyAddedException(Guid organizerId) : base($"Organizer with ID: '{organizerId}' was already added.")
        {
            OrganizerId = organizerId;
        }
    }
}