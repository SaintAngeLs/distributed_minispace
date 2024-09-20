using Convey.CQRS.Queries;
using MiniSpace.Services.Communication.Application.Dto;
using System;

namespace MiniSpace.Services.Communication.Application.Queries
{
    public class GetChatById : IQuery<ChatDto>
    {
        public Guid ChatId { get; }

        public GetChatById(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
