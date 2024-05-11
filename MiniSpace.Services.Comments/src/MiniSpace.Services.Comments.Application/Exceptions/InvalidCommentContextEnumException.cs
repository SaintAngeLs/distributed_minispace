using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class InvalidCommentContextEnumException : AppException
    {
        public override string Code { get; } = "invalid_commentcontext_enum";
        public string InvalidEnum { get; }

        public InvalidCommentContextEnumException(string invalidEnum) : base(
            $"String: {invalidEnum} cannot be parsed to valid CommentContext.")
        {
            InvalidEnum = invalidEnum;
        }
    }
}
