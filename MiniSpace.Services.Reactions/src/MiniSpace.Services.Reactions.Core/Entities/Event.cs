namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Event
    {
        public Guid Id { get; private set; }

        public Event(Guid id)
        {
            Id = id;
        }

    }
}
