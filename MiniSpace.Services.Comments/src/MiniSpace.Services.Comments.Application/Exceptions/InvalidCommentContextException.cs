using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class InvalidCommentContextException: AppException
    {
        public override string Code { get; } = "invalid_comment_context";
        public string Context { get; }

        public InvalidCommentContextException(string context) : base($"Invalid comment context: {context}.")
        {
            Context = context;
        }
    }
}