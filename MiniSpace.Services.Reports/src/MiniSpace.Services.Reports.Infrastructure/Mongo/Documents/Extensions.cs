using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Report AsEntity(this ReportDocument document)
            => new Report(document.Id, document.IssuerId, document.TargetId, document.TargetOwnerId, 
                document.ContextType, document.Category, document.Reason, document.State, document.CreatedAt);
        
        public static ReportDocument AsDocument(this Report entity)
            => new ReportDocument()
            {
                Id = entity.Id,
                IssuerId = entity.IssuerId,
                TargetId = entity.TargetId,
                TargetOwnerId = entity.TargetOwnerId,
                ContextType = entity.ContextType,
                Category = entity.Category,
                Reason = entity.Reason,
                State = entity.State,
                CreatedAt = entity.CreatedAt
            };
        
        public static ReportDto AsDto(this ReportDocument document)
            => new ReportDto()
            {
                Id = document.Id,
                IssuerId = document.IssuerId,
                TargetId = document.TargetId,
                TargetOwnerId = document.TargetOwnerId,
                ContextType = document.ContextType.ToString(),
                Category = document.Category.ToString(),
                Reason = document.Reason,
                Status = document.State.ToString(),
                CreatedAt = document.CreatedAt
            };
        
        public static Event AsEntity(this EventDocument document)
            => new Event(document.Id);

        public static EventDocument AsDocument(this Event entity)
            => new EventDocument
            {
                Id = entity.Id,
            };
        
        public static Post AsEntity(this PostDocument document)
            => new Post(document.Id);

        public static PostDocument AsDocument(this Post entity)
            => new PostDocument()
            {
                Id = entity.Id,
            };
        
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(document.Id);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument()
            {
                Id = entity.Id,
            };
    }    
}