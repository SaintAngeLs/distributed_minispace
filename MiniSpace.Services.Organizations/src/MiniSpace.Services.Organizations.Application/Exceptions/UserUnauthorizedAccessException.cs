namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class UserUnauthorizedAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_access";
        public string Resource { get; }

        public UserUnauthorizedAccessException(string resource)
            : base($"Unauthorized access to resource: '{resource}'.")
        {
            Resource = resource;
        }
    }
}
