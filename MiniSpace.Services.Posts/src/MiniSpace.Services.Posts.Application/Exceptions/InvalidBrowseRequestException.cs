namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class InvalidBrowseRequestException : AppException
    {
        public override string Code { get; } = "invalid_browse_request";

        public InvalidBrowseRequestException(string message) 
            : base(message)
        {
        }
    }
}
