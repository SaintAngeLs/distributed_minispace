namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventDescriptionException : AppException
    {
        public override string Code { get; } = "invalid_event_description";
        public string Description { get; }
        
        public InvalidEventDescriptionException(string description): base("Invalid event description. It cannot be empty or longer than 5000 characters.")
        {
            Description = description;
        }
    }
}