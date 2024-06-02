namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class UnauthorizedReportSearchAttemptException : AppException
    {
        public override string Code { get; } = "unauthorized_report_search_attempt";
        public Guid UserId { get; }
        public string Role { get; }

        public UnauthorizedReportSearchAttemptException(Guid userId, string role)
            : base($"Unauthorized report search attempt by user with ID: '{userId}' and role: {role}.")
        {
            UserId = userId;
            Role = role;
        }
    }
}