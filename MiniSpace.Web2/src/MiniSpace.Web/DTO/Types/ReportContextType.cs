using System;

namespace MiniSpace.Web.DTO.Types
{
    public enum ReportContextType
    {
        Event,
        Post,
        Comment,
        StudentProfile
    }

    public static class ReportContextTypeExtensions
    {
        public static string GetReportContextTypeText(ReportContextType reportContextType)
        {
            return reportContextType switch
            {
                ReportContextType.Event => "Event",
                ReportContextType.Post => "Post",
                ReportContextType.Comment => "Comment",
                ReportContextType.StudentProfile => "Student Profile",
                _ => throw new ArgumentOutOfRangeException(nameof(reportContextType), reportContextType, null)
            };
        }
    }
}