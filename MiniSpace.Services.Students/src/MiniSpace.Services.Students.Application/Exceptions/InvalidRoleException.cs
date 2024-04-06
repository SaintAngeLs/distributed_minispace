namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class InvalidRoleException : AppException
    {
        public override string Code { get; } = "invalid_role";
        
        public InvalidRoleException(Guid userId, string role, string requiredRole)
            : base($"Student account will not be created for the user with id: {userId} " +
                   $"due to the invalid role: {role} (required: {requiredRole}).")
        {
        }
    }    
}
