namespace MiniSpace.Services.Communication.Core.Events
{
    public class MessageAddedEvent : IDomainEvent
    {
        public Guid ChatId { get; }
        public Guid MessageId { get; }

        public MessageAddedEvent(Guid chatId, Guid messageId)
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
