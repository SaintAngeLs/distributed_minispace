using Convey.CQRS.Commands;
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Application.Commands
{
    public class CreateReport: ICommand
    {
        public Guid ReportId { get; }
        public Guid IssuerId { get; set; }
        public Guid TargetId { get; set; }
        public Guid TargetOwnerId { get; set; }
        public string ContextType { get; set; }
        public string Category { get; set; }
        public string Reason { get; private set; }

        public CreateReport(Guid reportId, Guid issuerId, Guid targetId, Guid targetOwnerId, string contextType, 
            string category, string reason)
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