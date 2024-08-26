using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Core.Entities
{
    public class OrganizationChats
    {
        public Guid OrganizationId { get; private set; }
        public List<Chat> Chats { get; private set; }

        public OrganizationChats(Guid organizationId)
        {
            OrganizationId = organizationId;
            Chats = new List<Chat>();
        }

        public void AddChat(Chat chat)
        {
            if (chat == null)
                throw new ArgumentNullException(nameof(chat));
            
            Chats.Add(chat);
        }

        public void RemoveChat(Guid chatId)
        {
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                Chats.Remove(chat);
            }
        }

        public Chat GetChatById(Guid chatId)
        {
            return Chats.FirstOrDefault(c => c.Id == chatId);
        }

        public bool ChatExists(Guid chatId)
        {
            return Chats.Any(c => c.Id == chatId);
        }

        public void UpdateChat(Chat updatedChat)
        {
            var existingChat = GetChatById(updatedChat.Id);
            if (existingChat != null)
            {
                Chats.Remove(existingChat);
                Chats.Add(updatedChat);
            }
        }
    }
}
