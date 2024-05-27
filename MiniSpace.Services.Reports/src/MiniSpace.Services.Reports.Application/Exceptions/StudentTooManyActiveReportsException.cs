namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class StudentTooManyActiveReportsException : AppException
    {
        public override string Code { get; } = "student_too_many_active_reports";
        public int ActiveReports { get; }
        public int MaxActiveReports { get; }

        public StudentTooManyActiveReportsException(int activeReports, int maxActiveReports)
            : base($"Student has too many active reports: {activeReports}. Max allowed is: {maxActiveReports}.")
        {
            ActiveReports = activeReports;
            MaxActiveReports = maxActiveReports;
        }
    }
}