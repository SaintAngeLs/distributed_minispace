namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class OrganizerNotFoundException : AppException
    {
        public override string Code { get; } = "organizer_not_found";
        public Guid OrganizerId { get; }

        public OrganizerNotFoundException(Guid organizerId) : base($"Organizer with ID: {organizerId} was not found.")
        {
            OrganizerId = organizerId;
        }
    }
}