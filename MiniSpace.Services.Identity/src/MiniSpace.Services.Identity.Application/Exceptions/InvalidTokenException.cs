namespace MiniSpace.Services.Identity.Application.Exceptions
{
    public class InvalidTokenException : AppException
    {
        public InvalidTokenException() : base("Invalid or expired token provided.")
        {
        }
    }
}
