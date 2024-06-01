using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
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