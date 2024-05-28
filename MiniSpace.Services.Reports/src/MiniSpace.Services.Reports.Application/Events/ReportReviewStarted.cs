using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
{
    public class ReportReviewStarted : IEvent
    {
        public Guid ReportId { get; }
        public Guid ReviewerId { get; }

        public ReportReviewStarted(Guid reportId, Guid reviewerId)
        {
            ReportId = reportId;
            ReviewerId = reviewerId;
        }
    }
}