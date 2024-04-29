namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class UnauthorizedAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_access";
        public string Role { get; }

        public UnauthorizedAccessException(string role) : base($"Unauthorized access. Required role: `{role}`")
        {
            Role = role;
        }
    }
}