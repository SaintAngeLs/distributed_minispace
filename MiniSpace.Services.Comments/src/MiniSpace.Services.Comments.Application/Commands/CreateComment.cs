using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class CreateComment : ICommand
    {
        public Guid CommentId { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
 

        public CreateComment(Guid commentId, Guid contextId, string commentContext, Guid studentId, Guid parentId,
            string comment)
        {
            CommentId = commentId == Guid.Empty ? Guid.NewGuid() : commentId;
            ContextId = contextId;
            CommentContext = commentContext;
            StudentId = studentId;
            ParentId = parentId;
            Comment = comment;
        }
    }
}