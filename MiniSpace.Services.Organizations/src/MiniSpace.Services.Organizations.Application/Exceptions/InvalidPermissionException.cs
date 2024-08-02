namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class InvalidPermissionException : AppException
    {
        public override string Code { get; } = "invalid_permission";
        public string Permission { get; }

        public InvalidPermissionException(string permission) : base($"Invalid permission: {permission}.")
        {
            Permission = permission;
        }
    }
}
