using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEventEngagementTypeException : AppException
    {
        public override string Code { get; } = "invalid_event_engagement_type";
        public string EngagementType { get; }

        public InvalidEventEngagementTypeException(string engagementType) 
            : base($"Invalid event engagement type: {engagementType}.")
        {
            EngagementType = engagementType;
        }
    }
}