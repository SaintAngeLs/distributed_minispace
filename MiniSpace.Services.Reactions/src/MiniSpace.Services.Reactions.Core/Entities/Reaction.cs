using System.Net.Mime;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Reaction : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        public string StudentFullName {get;private set;} 
        public ReactionType ReactionType { get; private set; } 
        public Guid ContentId { get; private set; }
        public ReactionContentType ContentType { get; private set; } 
        
        public Reaction(Guid reactionId, Guid studentId, string studentFullName, ReactionType reactionType,
            Guid contentId, ReactionContentType contentType) 
        {

            Id = reactionId;
            StudentId = studentId;
            StudentFullName = studentFullName;
            ReactionType = reactionType;
            ContentId = contentId;
            ContentType = contentType;
        }

        public static Reaction Create(Guid reactionId, Guid studentId, string studentFullName, ReactionType reactionType,
            Guid contentId, ReactionContentType contentType) 
        {
            return new Reaction(reactionId, studentId, studentFullName, reactionType, contentId, contentType);
        }


    }
}
