namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class UnauthorizedReportCreationAttemptException : AppException
    {
        public override string Code { get; } = "unauthorized_report_creation_attempt";
        public Guid StudentId { get; }
        public Guid IssuerId { get; }
        
        public UnauthorizedReportCreationAttemptException(Guid studentId, Guid issuerId) 
            : base($"Unauthorized report creation attempt for student with ID: {studentId}. Issuer ID: {issuerId}.")
        {
            StudentId = studentId;
            IssuerId = issuerId;
        }
    }
}