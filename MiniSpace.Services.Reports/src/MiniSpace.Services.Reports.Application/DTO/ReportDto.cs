namespace MiniSpace.Services.Reports.Application.DTO
{
    public class ReportDto
    {
        public Guid Id { get;  set; }
        public Guid IssuerId { get;  set; }
        public Guid TargetId { get;  set; }
        public string ContextType { get;  set; }
        public string Category { get;  set; }
        public string Reason { get;  set; }
        public string Status { get;  set; }
        public DateTime CreatedAt { get;  set; }
    }
}