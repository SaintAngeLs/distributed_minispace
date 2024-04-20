using System;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class EventCapacityExceededException : DomainException
    {
        public override string Code { get; } = "event_capacity_exceeded";
        public Guid Id { get; }
        public int Capacity { get; }
        public EventCapacityExceededException(Guid id, int capacity) : base($"Event with ID: {id} has exceeded its capacity." +
                                                                            $" Maximum capacity is: {capacity}")
        {
            Id = id;
            Capacity = capacity;
        }
    }
}