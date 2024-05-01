using System.Data;
using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class CreateReaction : ICommand
    {
        public Guid StudentId { get; }
        public string StudentFullName {get;}
        public string ReactionType { get; } 
        
        public Guid ContentId {get;}

        // Is the reaction related to event or post?
        public string ContentType { get; } 

        public CreateReaction(Guid studentId, Guid contentId,
                              string reactionType, string contentType)
        {
            StudentId = studentId;
            ContentId = contentId;
            ReactionType = reactionType;
            ContentType = contentType;
        }
    }    
}
