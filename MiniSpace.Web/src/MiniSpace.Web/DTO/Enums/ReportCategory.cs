using System;

namespace MiniSpace.Web.DTO.Enums
{
    public enum ReportCategory
    {
        Spam,
        HarassmentAndBullying,
        Violence,
        SexualContent,
        Misinformation,
        PrivacyViolations,
        IntellectualPropertyViolations,
        OtherViolations
    }
    
    public static class ReportCategoryExtensions
    {
        public static string GetReportCategoryText(ReportCategory reportCategory)
        {
            return reportCategory switch
            {
                ReportCategory.Spam => "Spam",
                ReportCategory.HarassmentAndBullying => "Harassment and bullying",
                ReportCategory.Violence => "Violence",
                ReportCategory.SexualContent => "Sexual content",
                ReportCategory.Misinformation => "Misinformation",
                ReportCategory.PrivacyViolations => "Privacy violations",
                ReportCategory.IntellectualPropertyViolations => "Intellectual property violations",
                ReportCategory.OtherViolations => "Other violations",
                _ => throw new ArgumentOutOfRangeException(nameof(reportCategory), reportCategory, null)
            };
        }
    }
}