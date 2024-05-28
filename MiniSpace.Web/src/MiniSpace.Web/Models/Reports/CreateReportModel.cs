using System;

namespace MiniSpace.Web.Models.Reports
{
    public class CreateReportModel
    {
        public Guid ReportId { get; set; }
        public Guid IssuerId { get; set; }
        public Guid TargetId { get; set; }
        public Guid TargetOwnerId { get; set; }
        public string ContextType { get; set; }
        public string Category { get; set; }
        public string Reason { get; set; }
        public bool CreatingSubmitted { get; set; }
    }
}
