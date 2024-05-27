namespace MiniSpace.Services.Email.Application.Exceptions
{
    public class InvalidEmailStatusException : AppException
    {
        public override string Code { get; } = "invalid_email_status";
        
        public InvalidEmailStatusException(string message)
            : base(message)
        {
        }
    }
}
