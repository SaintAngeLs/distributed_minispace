using Paralax.CQRS.Events;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class ChatCreated : IEvent
    {
        public Guid ChatId { get; set; }
        public List<Guid> ParticipantIds { get; set; }

        public ChatCreated(Guid chatId, List<Guid> participantIds)
        {
            ChatId = chatId;
            ParticipantIds = participantIds;
        }
    }
}
