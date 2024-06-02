using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventNameException : AppException
    {
        public override string Code { get; } = "invalid_event_name";
        public string Name { get; }
        
        public InvalidEventNameException(string name): base("Invalid event name. It cannot be empty or longer than 300 characters.")
        {
            Name = name;
        }
    }
}