namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class InvalidReportStateException : AppException
    {
        public override string Code { get; } = "invalid_report_state";
        public string State { get; }

        public InvalidReportStateException(string state) : base($"Invalid report state: {state}.")
        {
            State = state;
        }
    }
}