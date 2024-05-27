using Convey.CQRS.Commands;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class DeleteReport: ICommand
    {
        public Guid ReportId { get; set; }
    }
}