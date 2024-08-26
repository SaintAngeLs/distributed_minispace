namespace MiniSpace.Services.Communication.Core.Entities
{
    public class Chat : AggregateRoot
    {
        public Guid Id { get; private set; }
        public List<Guid> ParticipantIds { get; private set; }
        public List<Message> Messages { get; private set; }

        public Chat(List<Guid> participantIds)
        {
            Id = Guid.NewGuid();
            ParticipantIds = participantIds;
            Messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
            AddEvent(new Events.MessageAddedEvent(Id, message.Id));
        }
    }
}
