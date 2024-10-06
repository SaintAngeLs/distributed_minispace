using System.Reflection.Metadata;
using Paralax.CQRS.Commands;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class DeleteReaction : ICommand
    {
        public Guid ReactionId { get; }
        public DeleteReaction(Guid reactionId)
        {
            ReactionId = reactionId;
        }
    }    
}
