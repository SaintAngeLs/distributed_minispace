namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class EventNotFoundException : AppException
    {
        public override string Code { get; } = "event_not_found";
        public Guid Id { get; }

        public EventNotFoundException(Guid id) : base($"Event with id: {id} was not found.")
        {
            Id = id;
        }
    }    
}
