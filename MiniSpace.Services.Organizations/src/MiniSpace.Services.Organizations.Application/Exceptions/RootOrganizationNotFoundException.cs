namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class RootOrganizationNotFoundException : AppException
    {
        public override string Code { get; } = "root_organization_not_found";
        public Guid OrganizationId { get; }

        public RootOrganizationNotFoundException(Guid organizationId) : base($"Root organization with ID: '{organizationId}' was not found.")
        {
            OrganizationId = organizationId;
        }
    }
}