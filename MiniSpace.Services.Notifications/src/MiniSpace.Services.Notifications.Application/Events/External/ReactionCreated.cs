using System.Net.Mime;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Microsoft.AspNetCore.Connections;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("reactions")]
    public class ReactionCreated : IEvent
    {
        public Guid ReactionId {get;set;}
        public ReactionCreated(Guid reactionId)
        {
            ReactionId=reactionId;
        }
    }
}
