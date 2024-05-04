using MiniSpace.Services.Comments.Core.Exceptions;
using System;
using System.Collections.Generic;


namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment : AggregateRoot
    {
        public Guid ContextId { get; private set; }
        public CommentContext CommentContext { get; private set; }
        public Guid StudentId { get; private set; }
        public List<Guid> Likes { get; private set; }
        public Guid ParentId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        public Comment(Guid id, Guid contextId, CommentContext commentContext, Guid studentId, List<Guid> likes,
        Guid parentId, string textContent, DateTime createdAt, DateTime? lastUpdatedAt, bool isDeleted)
        {
            Id = id;
            ContextId = contextId;
            CommentContext = commentContext;
            StudentId = studentId;
            Likes = likes;
            ParentId = parentId;
            TextContent = textContent;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            IsDeleted = isDeleted;
        }

        public void Like(Guid studentId)
        {
            if(!Likes.Contains(studentId)) Likes.Add(studentId);
        }

        public void UnLike(Guid studentId)
        {
            if(Likes.Contains(studentId)) Likes.Remove(studentId);
        }

        public static Comment Create(AggregateId id, Guid contextId, CommentContext commentContext, Guid studentId, List<Guid> likes,
        Guid parentId, string textContent, DateTime createdAt)
        {
            CheckContent(id, textContent);

            return new Comment(id, contextId, commentContext, studentId, likes, parentId, textContent, createdAt, null , false);
        }

        public void Update(string textContent, DateTime now)
        {
            CheckContent(Id, textContent);

            TextContent = textContent;
            LastUpdatedAt = now;
        }

        private static void CheckContent(AggregateId id, string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent) || textContent.Length > 300)
            {
                throw new InvalidCommentContentException(id);
            }
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}