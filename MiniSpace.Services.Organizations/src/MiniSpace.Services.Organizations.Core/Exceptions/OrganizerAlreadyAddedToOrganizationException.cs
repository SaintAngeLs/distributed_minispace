namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class OrganizerAlreadyAddedToOrganizationException : DomainException
    {
        public override string Code { get; } = "organizer_already_added_to_organization";
        public Guid OrganizerId { get; }
        public Guid OrganizationId { get; }

        public OrganizerAlreadyAddedToOrganizationException(Guid organizerId, Guid organizationId)
            : base($"Organizer with ID: '{organizerId}' was already added to organization with ID: '{organizationId}'.")
        {
            OrganizerId = organizerId;
            OrganizationId = organizationId;
        }
    }
}