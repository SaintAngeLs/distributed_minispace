using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventFeeException : AppException
    {
        public override string Code { get; } = "invalid_event_fee";
        public decimal Fee { get; }

        public InvalidEventFeeException(decimal fee) : base($"Invalid event fee: {fee}. It must be between 0 and 1000.")
        {
            Fee = fee;
        }
    }
}