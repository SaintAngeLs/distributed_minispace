namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class ParentOrganizationNotFoundException : AppException
    {
        public override string Code { get; } = "parent_organization_not_found";
        public Guid ParentId { get; }

        public ParentOrganizationNotFoundException(Guid parentId) : base($"Parent organization with ID: '{parentId}' was not found.")
        {
            ParentId = parentId;
        }
    }
}