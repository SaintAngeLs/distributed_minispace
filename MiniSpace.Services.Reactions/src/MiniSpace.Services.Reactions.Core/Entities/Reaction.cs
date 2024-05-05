using System.Net.Mime;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Reaction(Guid studentId, ReactionType reactionType, ReactionContentType contentType,
                    Guid contentId) : AggregateRoot
    {
        public Guid StudentId { get; private set; } = studentId;
        public ReactionType ReactionType { get; private set; } = reactionType;
        public Guid ContentId { get; private set; } = contentId;
        public ReactionContentType ContentType { get; private set; } = contentType;

        public static Reaction Create(Guid studentId, ReactionType reactionType, ReactionContentType contentType,
                    Guid contentId) {
            return new Reaction(studentId, reactionType, contentType, contentId);
        }
    }
}