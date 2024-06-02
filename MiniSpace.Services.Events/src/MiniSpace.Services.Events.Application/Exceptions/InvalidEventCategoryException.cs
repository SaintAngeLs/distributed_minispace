using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
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