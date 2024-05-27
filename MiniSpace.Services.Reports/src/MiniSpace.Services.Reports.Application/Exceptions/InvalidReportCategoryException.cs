namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class InvalidReportCategoryException : AppException
    {
        public override string Code { get; } = "invalid_report_category";
        public string Category { get; }

        public InvalidReportCategoryException(string category) : base($"Invalid report category: {category}.")
        {
            Category = category;
        }
    }
}