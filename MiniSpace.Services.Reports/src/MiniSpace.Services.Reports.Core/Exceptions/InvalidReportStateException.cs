using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Exceptions
{
    public class InvalidReportStateException : DomainException
    {
        public override string Code { get; } = "invalid_report_state";
        public Guid ReportId { get; }
        public ReportState RequiredState { get; }
        public ReportState CurrentState { get; }
        
        public InvalidReportStateException(Guid reportId, ReportState requiredState, ReportState currentState) 
            : base($"Report with id: {reportId} has invalid state: {currentState}. Required state: {requiredState}")
        {
            ReportId = reportId;
            RequiredState = requiredState;
            CurrentState = currentState;
        }
    }
}