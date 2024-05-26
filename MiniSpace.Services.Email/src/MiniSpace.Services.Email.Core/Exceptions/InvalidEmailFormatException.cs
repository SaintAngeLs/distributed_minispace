namespace MiniSpace.Services.Email.Core.Exceptions
{
    public class InvalidEmailFormatException : DomainException
    {
        public override string Code { get; } = "invalid_email_format";
        public string Email { get; }

        public InvalidEmailFormatException(string email) 
            : base($"The email '{email}' has an invalid format.")
        {
            Email = email;
        }
    }
}
