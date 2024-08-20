namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class InvalidVisibilityStatusException : AppException
    {
        public override string Code { get; } = "invalid_visibility_status";
        public string Visibility { get; }

        public InvalidVisibilityStatusException(string visibility) 
            : base($"Invalid visibility status: '{visibility}'. It must be either 'Visible' or 'Invisible'.")
        {
            Visibility = visibility;
        }
    }
}
