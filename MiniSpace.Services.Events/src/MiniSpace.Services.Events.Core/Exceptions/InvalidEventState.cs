using System;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class InvalidEventState : DomainException
    {
        public override string Code { get; } = "invalid_event_state";
        public Guid EventId { get; }
        public State RequiredState { get; }
        public State CurrentState { get; }
        public InvalidEventState(Guid eventId, State requiredState, State currentState) 
            : base($"Event with id: {eventId} has invalid state: {currentState}. Required state: {requiredState}")
        {
            EventId = eventId;
            RequiredState = requiredState;
            CurrentState = currentState;
        }
    }
}