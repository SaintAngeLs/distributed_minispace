namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class InvalidReasonException : AppException
    {
        public override string Code { get; } = "invalid_reason";
        public string Reason { get; }
        
        public InvalidReasonException(string reason) : base($"Invalid reason: {reason}. It cannot be empty or over 1000 characters.")
        {
            Reason = reason;
        }
    }
}