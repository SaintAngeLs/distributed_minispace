using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Reports
{
    [Message("reports")]
    public class ReportDeleted: IEvent
    {
        public Guid ReportId { get; }
        public Guid IssuerId { get; }
        public Guid TargetId { get; }
        public Guid TargetOwnerId { get; }
        public string ContextType { get; }
        public string Category { get; }
        public string Reason { get; }
        public string State { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public Guid? ReviewerId { get; }
        
        public ReportDeleted(Guid reportId, Guid issuerId, Guid targetId, Guid targetOwnerId, 
            string contextType, string category, string reason, string state, 
            DateTime createdAt, DateTime updatedAt, Guid? reviewerId)
        {
            ReportId = reportId;
            IssuerId = issuerId;
            TargetId = targetId;
            TargetOwnerId = targetOwnerId;
            ContextType = contextType;
            Category = category;
            Reason = reason;
            State = state;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ReviewerId = reviewerId;
        }
    }
}