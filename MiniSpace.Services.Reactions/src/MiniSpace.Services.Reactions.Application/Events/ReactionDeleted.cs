using System.Net.Mime;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Connections;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Events
{
    public class ReactionDeleted : IEvent
    {
        public Guid ReactionId {get;set;}
        public ReactionDeleted(Guid reactionId)
        {
            ReactionId = reactionId;
        }
    }
}
