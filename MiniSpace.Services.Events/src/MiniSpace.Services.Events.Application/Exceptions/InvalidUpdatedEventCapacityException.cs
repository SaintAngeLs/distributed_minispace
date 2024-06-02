using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidUpdatedEventCapacityException : AppException
    {
        public override string Code { get; } = "invalid_updated_event_capacity";
        public int CurrentCapacity { get; }
        public int NewCapacity { get; }

        public InvalidUpdatedEventCapacityException(int currentCapacity, int newCapacity) 
            : base($"Invalid updated event capacity: {newCapacity}. It has to be greater than the current capacity: {currentCapacity}.")
        {
            CurrentCapacity = currentCapacity;
            NewCapacity = newCapacity;
        }
    }
}