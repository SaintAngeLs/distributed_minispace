using Convey.CQRS.Commands;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class RejectReport : ICommand
    {
        public Guid ReportId { get; }
        
        public RejectReport(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}