using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public static class OrganizationChatsExtensions
    {
        public static OrganizationChats AsEntity(this OrganizationChatsDocument document)
        {
            var organizationChats = new OrganizationChats(document.OrganizationId);
            foreach (var chatDocument in document.Chats)
            {
                var chat = chatDocument.AsEntity();
                organizationChats.AddChat(chat);
            }
            return organizationChats;
        }

        public static OrganizationChatsDocument AsDocument(this OrganizationChats entity)
        {
            return new OrganizationChatsDocument
            {
                Id = Guid.NewGuid(),
                OrganizationId = entity.OrganizationId,
                Chats = entity.Chats.Select(c => c.AsDocument()).ToList()
            };
        }
    }
}
