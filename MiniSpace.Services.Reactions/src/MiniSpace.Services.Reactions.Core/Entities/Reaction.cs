using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Reaction : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        public string StudentFullName { get; private set; }
        public ReactionType ReactionType { get; private set; }
        
        public Reaction(Guid studentId, string studentFullName, ReactionType reactionType)
        {
            StudentId = studentId;
            StudentFullName = studentFullName;
            ReactionType = reactionType;
        }

        public static Reaction Create(Guid studentId, string studentFullName, ReactionType reactionType)
        {
            return new Reaction(studentId, studentFullName, reactionType);
        }
    }    
}
