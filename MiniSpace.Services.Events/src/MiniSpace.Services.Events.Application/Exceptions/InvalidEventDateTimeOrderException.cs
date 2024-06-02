using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventDateTimeOrderException(DateTime earlierDate, DateTime laterDate, string earlierDateProperty, string laterDateProperty)
        : AppException($"Event DateTime property `{earlierDateProperty}`:`{earlierDate}` is later than `{laterDateProperty}`:`{laterDate}`.")
    {
        public override string Code { get; } = "invalid_event_date_time";
        public string EarlierProperty { get; } = earlierDateProperty;
        public string LaterProperty { get; } = laterDateProperty;
        public DateTime EarlierDate { get; } = earlierDate;
        public DateTime LaterDate { get; } = laterDate;
    }
}