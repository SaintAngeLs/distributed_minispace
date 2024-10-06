using System;

namespace Astravent.Web.Wasm.Areas.Reports.CommandsDto
{
    public class CreateReportCommand
    {
        public Guid ReportId { get; }
        public Guid IssuerId { get; set; }
        public Guid TargetId { get; set; }
        public Guid TargetOwnerId { get; set; }
        public string ContextType { get; set; }
        public string Category { get; set; }
        public string Reason { get; private set; }

        public CreateReportCommand(Guid reportId, Guid issuerId, Guid targetId, Guid targetOwnerId, 
            string contextType, string category, string reason)
        {
            ReportId = reportId == Guid.Empty ? Guid.NewGuid() : reportId;
            IssuerId = issuerId;
            TargetId = targetId;
            TargetOwnerId = targetOwnerId;
            ContextType = contextType;
            Category = category;
            Reason = reason;
        }
    }
}
