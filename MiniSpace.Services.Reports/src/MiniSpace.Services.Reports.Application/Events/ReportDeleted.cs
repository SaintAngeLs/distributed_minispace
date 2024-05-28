using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
{
    public class ReportDeleted: IEvent
    {
        public Guid ReportId { get; }
        
        public ReportDeleted(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}