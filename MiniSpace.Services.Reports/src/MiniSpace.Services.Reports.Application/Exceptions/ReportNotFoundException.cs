namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class ReportNotFoundException : AppException
    {
        public override string Code { get; } = "report_not_found";
        public Guid ReportId { get; }

        public ReportNotFoundException(Guid reportId) : base($"Report with ID: '{reportId}' was not found.")
        {
            ReportId = reportId;
        }
    }
}