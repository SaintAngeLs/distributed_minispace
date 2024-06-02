using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventStateException : AppException
    {
        public override string Code { get; } = "invalid_event_state";
        public string State { get; }

        public InvalidEventStateException(string state) : base($"Event State property is invalid: {state}.")
        {
            State = state;
        }
    }
}