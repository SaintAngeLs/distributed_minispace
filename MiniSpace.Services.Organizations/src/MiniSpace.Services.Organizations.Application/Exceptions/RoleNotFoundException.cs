namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class RoleNotFoundException : AppException
    {
        public override string Code { get; } = "role_not_found";
        public Guid RoleId { get; }
        public string RoleName { get; }

        public RoleNotFoundException(Guid roleId) : base($"Role with id '{roleId}' was not found.")
        {
            RoleId = roleId;
        }

        public RoleNotFoundException(string roleName) : base($"Role with name '{roleName}' was not found.")
        {
            RoleName = roleName;
        }
    }
}
