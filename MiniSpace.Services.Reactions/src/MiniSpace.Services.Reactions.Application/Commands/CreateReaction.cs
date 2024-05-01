using System.Data;
using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class CreateReaction : ICommand
    {
        public Guid StudentId { get; }
        public string ReactionType { get; } 
        
        public string Content {get;}

        // Is the reaction related to event or post?
        public string ContentType { get; } 

        public CreateReaction(Guid studentId, string content,
                              string reactionType, string contentType)
        {
            StudentId = studentId;
            Content = content;
            ReactionType = reactionType;
            ContentType = contentType;
        }
    }    
}
