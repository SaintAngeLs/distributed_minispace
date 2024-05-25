namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class ParentOfOrganizationNotFoundException : DomainException
    {
        public override string Code { get; } = "parent_of_organization_not_found";
        public Guid ChildId { get; }

        public ParentOfOrganizationNotFoundException(Guid childId) : base(
            $"Parent organization was not found for child organization with ID: '{childId}'.")
        {
            ChildId = childId;
        }
    }
}