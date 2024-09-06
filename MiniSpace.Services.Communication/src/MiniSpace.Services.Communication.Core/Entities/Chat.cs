namespace MiniSpace.Services.Communication.Core.Entities
{
    public class Chat : AggregateRoot
    {
        public Guid Id { get; private set; }
        public List<Guid> ParticipantIds { get; private set; }
        public List<Message> Messages { get; private set; }

        // Constructor for creating a new chat with a new Id
        public Chat(List<Guid> participantIds)
        {
            Id = Guid.NewGuid();
            ParticipantIds = participantIds;
            Messages = new List<Message>();
        }

        // Constructor for loading an existing chat from the database
        public Chat(Guid id, List<Guid> participantIds, List<Message> messages)
        {
            Id = id;
            ParticipantIds = participantIds;
            Messages = messages;
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
            AddEvent(new Events.MessageAddedEvent(Id, message.Id));
        }
    }
}
