using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
{
    public class ReportRejected: IEvent
    {
        public Guid ReportId { get; }
        
        public ReportRejected(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}