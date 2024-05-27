using Convey.CQRS.Commands;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class ResolveReport : ICommand
    {
        public Guid ReportId { get; }
        
        public ResolveReport(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}