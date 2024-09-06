using Convey.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Queries
{
    public class GetMessagesForChat : IQuery<IEnumerable<MessageDto>>
    {
        public Guid ChatId { get; }

        public GetMessagesForChat(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
