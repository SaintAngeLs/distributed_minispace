using System;

namespace MiniSpace.Services.Communication.Core.Entities
{
    public class Message
    {
        public Guid Id { get; private set; }
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public Guid ChatId { get; private set; }
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }
        public MessageType Type { get; private set; }
        public MessageStatus Status { get; private set; }

        // Constructor for creating a new message
        public Message(Guid chatId, Guid senderId, Guid receiverId, string content, MessageType type)
        {
            Id = Guid.NewGuid(); // Generate a new ID for a new message
            ChatId = chatId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Timestamp = DateTime.UtcNow;
            Type = type;
            Status = MessageStatus.Sent;
        }

        // Constructor for loading an existing message from the database
        public Message(Guid id, Guid chatId, Guid senderId, Guid receiverId, string content, DateTime timestamp, MessageType type, MessageStatus status)
        {
            Id = id; // Use the existing ID from the database
            ChatId = chatId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Timestamp = timestamp;
            Type = type;
            Status = status;
        }

        public void MarkAsRead()
        {
            Status = MessageStatus.Read;
        }

        public void MarkAsUnread()
        {
            if (Status == MessageStatus.Read)
            {
                Status = MessageStatus.Unread;
            }
        }

        public void MarkAsDeleted()
        {
            Status = MessageStatus.Deleted;
        }
    }
}
