using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Application.DTO
{
    public class ReportDto
    {
        public Guid Id { get;  set; }
        public Guid IssuerId { get;  set; }
        public Guid TargetId { get;  set; }
        public Guid TargetOwnerId { get;  set; }
        public string ContextType { get;  set; }
        public string Category { get;  set; }
        public string Reason { get;  set; }
        public string State { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }
        public Guid? ReviewerId { get;  set; }

        public ReportDto()
        {
            
        }
        
        public ReportDto(Report report)
        {
            Id = report.Id;
            IssuerId = report.IssuerId;
            TargetId = report.TargetId;
            TargetOwnerId = report.TargetOwnerId;
            ContextType = report.ContextType.ToString();
            Category = report.Category.ToString();
            Reason = report.Reason;
            State = report.State.ToString();
            CreatedAt = report.CreatedAt;
            UpdatedAt = report.UpdatedAt;
            ReviewerId = report.ReviewerId;
        }
    }
}