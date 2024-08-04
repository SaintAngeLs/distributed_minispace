namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class RoleAlreadyExistsException : DomainException
    {
        public override string Code { get; } = "role_already_exists";
        public string RoleName { get; }

        public RoleAlreadyExistsException(string roleName) : base($"Role '{roleName}' already exists.")
        {
            RoleName = roleName;
        }
    }
}
