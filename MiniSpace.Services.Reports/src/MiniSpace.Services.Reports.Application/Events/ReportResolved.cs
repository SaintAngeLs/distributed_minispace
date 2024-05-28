using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
{
    public class ReportResolved: IEvent
    {
        public Guid ReportId { get; }

        public ReportResolved(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}