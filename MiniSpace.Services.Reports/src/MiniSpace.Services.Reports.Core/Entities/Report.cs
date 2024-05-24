using MiniSpace.Services.Reports.Core.Exceptions;

namespace MiniSpace.Services.Reports.Core.Entities
{
    public class Report : AggregateRoot
    {
        public Guid IssuerId { get; private set; }
        public Guid TargetId { get; private set; }
        public Guid TargetOwnerId { get; private set; }
        public ContextType ContextType { get; private set; }
        public ReportCategory Category { get; private set; }
        public string Reason { get; private set; }
        public ReportState State { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Guid? ReviewerId { get; private set; }
        
        public Report(Guid id, Guid issuerId, Guid targetId, Guid targetOwnerId, ContextType contextType, 
            ReportCategory category, string reason, ReportState state, DateTime createdAt, DateTime? updatedAt = null,
            Guid? reviewerId = null)
        {
            Id = id;
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

        public static Report Create(Guid id, Guid issuerId, Guid targetId, Guid targetOwnerId, ContextType contextType,
            ReportCategory category, string reason, DateTime now)   
            => new Report(id, issuerId, targetId, targetOwnerId, contextType, category, reason, ReportState.Submitted,
                now);

        public void Cancel(DateTime now)
        {
            if (State != ReportState.Submitted)
            {
                throw new InvalidReportStateException(Id, ReportState.Submitted, State);
            }
            State = ReportState.Cancelled;
            UpdatedAt = now;
        }
        
        public void StartReview(Guid reviewerId, DateTime now)
        {
            if (State != ReportState.Submitted)
            {
                throw new InvalidReportStateException(Id, ReportState.Submitted, State);
            }
            State = ReportState.UnderReview;
            UpdatedAt = now;
            ReviewerId = reviewerId;
        }
        
        public void Resolve(DateTime now)
        {
            if (State != ReportState.UnderReview)
            {
                throw new InvalidReportStateException(Id, ReportState.UnderReview, State);
            }
            State = ReportState.Resolved;
            UpdatedAt = now;
        }
        
        public void Reject(DateTime now)
        {
            if (State != ReportState.UnderReview)
            {
                throw new InvalidReportStateException(Id, ReportState.UnderReview, State);
            }
            State = ReportState.Rejected;
            UpdatedAt = now;
        }
    }
}