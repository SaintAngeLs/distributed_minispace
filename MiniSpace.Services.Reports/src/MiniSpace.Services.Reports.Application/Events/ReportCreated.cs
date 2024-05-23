using Convey.CQRS.Events;

namespace MiniSpace.Services.Reports.Application.Events
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