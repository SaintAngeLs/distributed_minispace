using System;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class InvalidRequestStateException : AppException
    {
        public override string Code { get; } = "invalid_request_state";
        public RequestState State { get; }

        public InvalidRequestStateException(RequestState state, string message)
            : base(message)
        {
            State = state;
        }
    }
}
