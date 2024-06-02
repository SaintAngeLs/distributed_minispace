using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventDateTimeException(string element, string value)
        : AppException($"Event DateTime property `{element}` is invalid: {value}.")
    {
        public override string Code { get; } = "invalid_event_date_time";
        public string Element { get; } = element;
        public string Value { get; } = value;
    }
}
