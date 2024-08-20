using MiniSpace.Services.Comments.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment : AggregateRoot
    {
        private ISet<Guid> _likes = new HashSet<Guid>();
        private ISet<Reply> _replies = new HashSet<Reply>();

        public Guid ContextId { get; private set; }
        public CommentContext CommentContext { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ParentId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public DateTime LastReplyAt { get; private set; }
        public int RepliesCount => _replies.Count;
        public bool IsDeleted { get; private set; }

        public IEnumerable<Guid> Likes
        {
            get => _likes;
            private set => _likes = new HashSet<Guid>(value);
        }

        public IEnumerable<Reply> Replies
        {
            get => _replies;
            private set => _replies = new HashSet<Reply>(value);
        }

        public Comment(Guid id, Guid contextId, CommentContext commentContext, Guid userId,
            IEnumerable<Guid> likes, Guid parentId, string textContent, DateTime createdAt, DateTime lastUpdatedAt, 
            DateTime lastReplyAt, IEnumerable<Reply> replies, bool isDeleted)
        {
            Id = id;
            ContextId = contextId;
            CommentContext = commentContext;
            UserId = userId;
            Likes = likes;
            ParentId = parentId;
            TextContent = textContent;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            LastReplyAt = lastReplyAt;
            Replies = replies;
            IsDeleted = isDeleted;
        }

        public void Like(Guid userId)
        {
            if (Likes.Any(id => id == userId))
            {
                throw new UserAlreadyLikesCommentException(userId);
            }
            _likes.Add(userId);
        }

        public void UnLike(Guid userId)
        {
            if (Likes.All(id => id != userId))
            {
                throw new UserNotLikeCommentException(userId, Id);
            }
            _likes.Remove(userId);
        }

        public static Comment Create(AggregateId id, Guid contextId, CommentContext commentContext, Guid userId, 
            Guid parentId, string textContent, DateTime createdAt)
        {
            CheckContent(id, textContent);

            return new Comment(id, contextId, commentContext, userId, new List<Guid>(), parentId, textContent, 
                createdAt, createdAt, createdAt, new List<Reply>(), false);
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

        public void AddReply(Guid replyId, Guid userId, string textContent, DateTime now)
        {
            var reply = new Reply(replyId, userId, Id, textContent, now);
            _replies.Add(reply);
            LastReplyAt = now;
        }

        public void RemoveReply(Guid replyId)
        {
            var reply = _replies.FirstOrDefault(r => r.Id == replyId);
            if (reply != null)
            {
                _replies.Remove(reply);
            }
        }
    }
}
