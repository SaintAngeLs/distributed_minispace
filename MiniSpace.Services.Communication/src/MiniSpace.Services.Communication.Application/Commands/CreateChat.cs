using Paralax.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class CreateChat : ICommand
    {
        public Guid ChatId { get; }
        public List<Guid> ParticipantIds { get; }
        public string ChatName { get; }

        public CreateChat(Guid chatId, List<Guid> participantIds, string chatName = null)
        {
            ChatId = chatId;
            ParticipantIds = participantIds ?? new List<Guid>();
            ChatName = chatName;
        }
    }
}
