namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class InvalidRoleException : AppException
    {
        public override string Code { get; } = "invalid_role";
        
        public InvalidRoleException(Guid userId, string role, string requiredRole)
            : base($"User account will not be created for the user with id: {userId} " +
                   $"due to the invalid role: {role} (required: {requiredRole}).")
        {
        }
    }    
}
