using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Core.Entities
{
    public class UserMessages
    {
        public Guid UserId { get; private set; }
        private List<Message> _messages;

        public IEnumerable<Message> Messages => _messages.AsReadOnly();

        public UserMessages(Guid userId)
        {
            UserId = userId;
            _messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            if (message != null)
            {
                _messages.Add(message);
            }
        }

        public void RemoveMessage(Guid messageId)
        {
            _messages.RemoveAll(m => m.Id == messageId);
        }

        public void MarkMessageAsRead(Guid messageId)
        {
            var message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                message.MarkAsRead();
            }
        }

        public void MarkMessageAsUnread(Guid messageId)
        {
            var message = _messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                message.MarkAsUnread();
            }
        }

        public IEnumerable<Message> GetMessagesForChat(Guid chatId)
        {
            return _messages.Where(m => m.ChatId == chatId);
        }
    }
}
