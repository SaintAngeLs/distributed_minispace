using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
{
    public class ReportCancelled : IEvent
    {
        public Guid ReportId { get; }
        
        public ReportCancelled(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}