using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class CreateComment : ICommand
    {
        public Guid Id { get; set; }
        public Guid ContextId { get; set; }
        public Guid StudentId { get; set; }
        public List<Guid> Likes { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
 

        public CreateComment(Guid id, Guid contextId, Guid studentId, List<Guid> likes,
            Guid parentId, string comment, DateTime createdAt)
        {
            Id = id;
            ContextId = contextId;
            StudentId = studentId;
            Likes = likes;
            ParentId = parentId;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }
}