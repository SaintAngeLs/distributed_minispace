using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    public class ReportCreated: IEvent
    {
        public Guid ReportId { get; }

        public ReportCreated(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}