namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Event(Guid id)
    {
        public Guid Id { get; private set; } = id;
    }
}
