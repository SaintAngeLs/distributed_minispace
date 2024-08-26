using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public static class UserChatsExtensions
    {
        public static UserChats AsEntity(this UserChatsDocument document)
        {
            var userChats = new UserChats(document.UserId);
            foreach (var chatDocument in document.Chats)
            {
                var chat = chatDocument.AsEntity();
                userChats.AddChat(chat);
            }
            return userChats;
        }

        public static UserChatsDocument AsDocument(this UserChats entity)
        {
            return new UserChatsDocument
            {
                Id = Guid.NewGuid(),
                UserId = entity.UserId,
                Chats = entity.Chats.Select(c => c.AsDocument()).ToList()
            };
        }
    }
}
