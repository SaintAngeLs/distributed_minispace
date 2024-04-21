using System.Data;
using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class CreateReaction : ICommand
    {
        public Guid StudentId { get; }
        public string StudentFullName { get; }

        public string ReactionType { get; } 
        
        // Is the reaction related to event or post?
        public string ContentType { get; } 

        public CreateReaction(Guid studentId, string studentFullName,
                              string reactionType, string contentType)
        {
            StudentId = studentId;
            StudentFullName = studentFullName;
            ReactionType = reactionType;
            ContentType = contentType;
        }
    }    
}
