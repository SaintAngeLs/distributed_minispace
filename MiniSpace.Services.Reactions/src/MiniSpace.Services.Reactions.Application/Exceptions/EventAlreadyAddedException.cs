namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class EventAlreadyAddedException : AppException
    {
        public override string Code { get; } = "event_already_added";
        public Guid EventId { get; }
    
        public EventAlreadyAddedException(Guid eventId)
            : base($"Event with id: {eventId} was already added.")
        {
            EventId = eventId;
        }
    }    
}
