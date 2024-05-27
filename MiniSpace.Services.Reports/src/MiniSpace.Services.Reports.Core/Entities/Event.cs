namespace MiniSpace.Services.Reports.Core.Entities
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