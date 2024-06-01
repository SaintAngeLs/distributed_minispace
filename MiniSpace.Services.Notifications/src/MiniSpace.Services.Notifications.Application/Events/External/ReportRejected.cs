using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
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