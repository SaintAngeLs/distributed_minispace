using System;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Reaction
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ContentId { get; private set; }
        public string Type { get; private set; }
        public string ContentType { get; private set; }
        public string TargetType { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Reaction() { }

        private Reaction(Guid id, Guid userId, string type, Guid contentId, string contentType, string targetType)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Reaction type cannot be null or empty.", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("Content type cannot be null or empty.", nameof(contentType));
            }

            if (string.IsNullOrWhiteSpace(targetType))
            {
                throw new ArgumentException("Target type cannot be null or empty.", nameof(targetType));
            }

            Id = id != Guid.Empty ? id : throw new ArgumentException("Reaction ID cannot be empty.", nameof(id));
            UserId = userId != Guid.Empty ? userId : throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            ContentId = contentId != Guid.Empty ? contentId : throw new ArgumentException("Content ID cannot be empty.", nameof(contentId));
            Type = type;
            ContentType = contentType;
            TargetType = targetType;
            CreatedAt = DateTime.UtcNow;
        }

        public static Reaction Create(Guid id, Guid userId, string type, Guid contentId, string contentType, string targetType)
        {
            return new Reaction(id, userId, type, contentId, contentType, targetType);
        }

        public void UpdateReactionType(string newType)
        {
            if (string.IsNullOrWhiteSpace(newType))
            {
                throw new ArgumentException("Reaction type cannot be null or empty.", nameof(newType));
            }

            Type = newType;
        }
    }
}
