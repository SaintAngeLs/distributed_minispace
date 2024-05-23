using Convey.Types;
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class ReportDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid IssuerId { get; set; }
        public Guid TargetId { get; set; }
        public Guid TargetOwnerId { get; set; }
        public ContextType ContextType { get; set; }
        public ReportCategory Category { get; set; }
        public string Reason { get; set; }
        public ReportState State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}