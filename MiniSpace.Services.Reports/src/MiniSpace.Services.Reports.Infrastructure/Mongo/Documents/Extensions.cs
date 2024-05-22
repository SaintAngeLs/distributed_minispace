using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Report AsEntity(this ReportDocument document)
            => new Report(document.Id, document.IssuerId, document.TargetId, document.ContextType, document.Category,
                document.Reason, document.Status, document.CreatedAt);
        
        public static ReportDocument AsDocument(this Report entity)
            => new ReportDocument()
            {
                Id = entity.Id,
                IssuerId = entity.IssuerId,
                TargetId = entity.TargetId,
                ContextType = entity.ContextType,
                Category = entity.Category,
                Reason = entity.Reason,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        
        public static ReportDto AsDto(this ReportDocument document)
            => new ReportDto()
            {
                Id = document.Id,
                IssuerId = document.IssuerId,
                TargetId = document.TargetId,
                ContextType = document.ContextType.ToString(),
                Category = document.Category.ToString(),
                Reason = document.Reason,
                Status = document.Status.ToString(),
                CreatedAt = document.CreatedAt
            };
    }    
}