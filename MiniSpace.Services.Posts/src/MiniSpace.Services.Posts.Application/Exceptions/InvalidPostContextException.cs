using System;
using MiniSpace.Services.Posts.Application.Exceptions;

namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class InvalidPostContextException : AppException
    {
        public override string Code { get; } = "invalid_post_context";
        public string Context { get; }

        public InvalidPostContextException(string context) 
            : base($"The post context '{context}' is invalid.")
        {
            Context = context;
        }
    }
}
