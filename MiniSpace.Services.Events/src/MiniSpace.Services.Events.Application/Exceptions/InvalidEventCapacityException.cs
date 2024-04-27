namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventCapacityException : AppException
    {
        public override string Code { get; } = "invalid_event_capacity";
        public int Capacity { get; }

        public InvalidEventCapacityException(int capacity) : base($"Invalid event capacity: {capacity}. It must be between 1 and 1000.")
        {
            Capacity = capacity;
        }
    }
}