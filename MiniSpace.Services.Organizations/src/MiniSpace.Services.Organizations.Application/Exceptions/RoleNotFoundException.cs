namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class RoleNotFoundException : AppException
    {
        public override string Code { get; } = "role_not_found";
        public Guid RoleId { get; }

        public RoleNotFoundException(Guid roleId) : base($"Role with id '{roleId}' was not found.")
        {
            RoleId = roleId;
        }
    }
}
