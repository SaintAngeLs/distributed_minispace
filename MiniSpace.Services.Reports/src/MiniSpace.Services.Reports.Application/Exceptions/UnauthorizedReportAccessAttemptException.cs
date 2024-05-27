namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class UnauthorizedReportAccessAttemptException : AppException
    {
        public override string Code => "unauthorized_report_access_attempt";
        public Guid ReportId { get; }
        public Guid StudentId { get; }
        
        public UnauthorizedReportAccessAttemptException(Guid reportId, Guid studentId) 
            : base($"Unauthorized report access attempt with ID: {reportId} by student with ID: {studentId}.")
        {
            ReportId = reportId;
            StudentId = studentId;
        }
    }
}