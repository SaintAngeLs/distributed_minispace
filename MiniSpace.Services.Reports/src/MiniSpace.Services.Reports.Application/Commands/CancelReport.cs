using Convey.CQRS.Commands;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class CancelReport: ICommand
    {
        public Guid ReportId { get; }

        public CancelReport(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}