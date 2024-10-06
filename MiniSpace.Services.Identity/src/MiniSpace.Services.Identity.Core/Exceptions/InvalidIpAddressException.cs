namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class InvalidIpAddressException : DomainException
    {
        public override string Code { get; } = "invalid_ip_address";

        public InvalidIpAddressException() : base("IP address cannot be null or empty.")
        {
        }

        public InvalidIpAddressException(string message) : base(message)
        {
        }
    }
}
