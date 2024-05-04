using MiniSpace.Services.Comments.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment : AggregateRoot
    {
        private ISet<Guid> _likes = new HashSet<Guid>();
        public Guid ContextId { get; private set; }
        public CommentContext CommentContext { get; private set; }
        public Guid StudentId { get; private set; }
        public string StudentName { get; private set; }
        public Guid ParentId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public DateTime LastReplyAt { get; private set; }
        public int RepliesCount { get; private set; }
        public bool IsDeleted { get; private set; }
        
        public IEnumerable<Guid> Likes
        {
            get => _likes;
            private set => _likes = new HashSet<Guid>(value);
        }

        public Comment(Guid id, Guid contextId, CommentContext commentContext, Guid studentId, string studentName,
            IEnumerable<Guid> likes, Guid parentId, string textContent, DateTime createdAt, DateTime lastUpdatedAt, 
            DateTime lastReplyAt, int repliesCount, bool isDeleted)
        {
            Id = id;
            ContextId = contextId;
            CommentContext = commentContext;
            StudentId = studentId;
            StudentName = studentName;
            Likes = likes;
            ParentId = parentId;
            TextContent = textContent;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            LastReplyAt = lastReplyAt;
            RepliesCount = repliesCount;
            IsDeleted = isDeleted;
        }

        public void Like(Guid studentId)
        {
            if (Likes.Any(id => id == studentId))
            {
                throw new StudentAlreadyLikesCommentException(studentId);
            }
            _likes.Add(studentId);
        }

        public void UnLike(Guid studentId)
        {
            if (Likes.All(id => id != studentId))
            {
                throw new StudentNotLikeCommentException(studentId);
            }
            _likes.Remove(studentId);
        }

        public static Comment Create(AggregateId id, Guid contextId, CommentContext commentContext, Guid studentId, 
            string studentName, List<Guid> likes, Guid parentId, string textContent, DateTime createdAt)
        {
            CheckContent(id, textContent);

            return new Comment(id, contextId, commentContext, studentId, studentName, likes, parentId, textContent, 
                createdAt, createdAt, createdAt, 0,false);
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

        public void Delete()
        {
            IsDeleted = true;
            TextContent = "";
        }
        
        public void AddReply(DateTime now)
        {
            RepliesCount++;
            LastReplyAt = now;
        }
    }
}