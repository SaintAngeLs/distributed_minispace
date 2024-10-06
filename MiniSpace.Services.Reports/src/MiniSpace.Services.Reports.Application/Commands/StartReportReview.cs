using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class StartReportReview: ICommand
    {
        public Guid ReportId { get; }
        public Guid ReviewerId { get; set; }

        public StartReportReview(Guid reportId, Guid reviewerId)
        {
            ReportId = reportId;
            ReviewerId = reviewerId;
        }
    }
}