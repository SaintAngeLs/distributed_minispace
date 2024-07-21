
namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; } // can be a user or a group name
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType Type { get; set; }

        public Message(string sender, string receiver, string content, MessageType type)
        {
            Id = Guid.NewGuid();
            Sender = sender;
            Receiver = receiver;
            Content = content;
            Timestamp = DateTime.UtcNow;
            Type = type;
        }
    }
}
