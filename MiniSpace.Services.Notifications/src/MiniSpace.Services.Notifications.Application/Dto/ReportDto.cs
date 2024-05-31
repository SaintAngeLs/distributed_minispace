using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.DTO
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
        public DateTime UpdatedAt { get;  set; }
        public Guid? ReviewerId { get;  set; }
    }
}