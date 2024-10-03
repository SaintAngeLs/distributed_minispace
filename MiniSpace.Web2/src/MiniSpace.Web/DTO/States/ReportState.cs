using System;

namespace MiniSpace.Web.DTO.States
{
    public enum ReportState
    {
        Submitted,
        UnderReview,
        Resolved,
        Rejected,
        Cancelled
    }
    
    public static class ReportStateExtensions
    {
        public static string GetReportStateText(ReportState reportState)
        {
            return reportState switch
            {
                ReportState.Submitted => "Submitted",
                ReportState.UnderReview => "Under review",
                ReportState.Resolved => "Resolved",
                ReportState.Rejected => "Rejected",
                ReportState.Cancelled => "Cancelled",
                _ => throw new ArgumentOutOfRangeException(nameof(reportState), reportState, null)
            };
        }
    }
}