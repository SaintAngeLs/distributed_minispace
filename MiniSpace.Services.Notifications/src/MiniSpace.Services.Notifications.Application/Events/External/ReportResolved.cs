using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
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