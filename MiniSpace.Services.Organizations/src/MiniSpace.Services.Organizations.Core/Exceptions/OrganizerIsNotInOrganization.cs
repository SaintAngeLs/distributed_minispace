namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class OrganizerIsNotInOrganization : DomainException
    {
        public override string Code { get; } = "organizer_is_not_in_organization";
        public Guid OrganizerId { get; }
        public Guid OrganizationId { get; }

        public OrganizerIsNotInOrganization(Guid organizerId, Guid organizationId) 
            : base($"Organizer with ID: '{organizerId}' is not in organization with ID: '{organizationId}'.")
        {
            OrganizerId = organizerId;
            OrganizationId = organizationId;
        }
    }
}