using System.Data;
using System.Threading.Tasks.Dataflow;
using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class CreateReaction : ICommand
    {
        public Guid ReactionId {get;}
        public Guid StudentId { get; }
        public string ReactionType { get; } 
        public Guid ContentId {get;}
        public string ContentType { get; } 

        public CreateReaction(Guid reactionId, Guid studentId,
                            Guid contentId,
                              string reactionType, string contentType)
        {
            ReactionId = reactionId == Guid.Empty ? Guid.NewGuid() : reactionId;
            StudentId = studentId;
            ContentId = contentId;
            ReactionType = reactionType;
            ContentType = contentType;
        }
    }    
}
