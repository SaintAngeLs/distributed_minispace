namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventCategoryException : AppException
    {
        public override string Code { get; } = "invalid_event_category";
        public string Category { get; }

        public InvalidEventCategoryException(string category) : base($"Invalid event category: {category}")
        {
            Category = category;
        }
    }
}