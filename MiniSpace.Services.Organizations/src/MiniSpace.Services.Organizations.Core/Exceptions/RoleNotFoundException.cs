namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class RoleNotFoundException : DomainException
    {
        public override string Code { get; } = "role_not_found";
        public Guid RoleId { get; }
        public String RoleName { get; }

        public RoleNotFoundException(Guid roleId) : base($"Role with ID: '{roleId}' was not found.")
        {
            RoleId = roleId;
        }
        public RoleNotFoundException(string roleName) : base($"Role with name: '{roleName}' was not found.")
        {
            RoleName = roleName;
        }
    }
}
