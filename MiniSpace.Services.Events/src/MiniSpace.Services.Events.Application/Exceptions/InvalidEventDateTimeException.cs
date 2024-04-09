namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventDateTimeException : AppException
    {
        public override string Code { get; } = "invalid_event_date_time";
        public string Element { get; }
        public string Value { get; }

        public InvalidEventDateTimeException(string element, string value) 
            : base($"Event DateTime property `{element}` is invalid: {value}.")
        {
            Element = element;
            Value = value;
        }
    }
}
