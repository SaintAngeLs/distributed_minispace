using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Reaction AsEntity(this ReactionDocument document)
            => new Reaction(document.StudentId, document.StudentFullName, document.Type, document.ContentType, document.ContentId);

        public static ReactionDocument AsDocument(this Reaction entity)
            => new ReactionDocument()
            {
                Id = entity.Id,
                ContentId = entity.ContentId,
                ContentType = entity.ContentType,
                Type = entity.ReactionType,
                StudentFullName = entity.StudentFullName,
                StudentId = entity.StudentId
            };

        public static ReactionDto AsDto(this ReactionDocument document)
            => new ReactionDto()
            {
                Id = document.Id,
                ContentId = document.ContentId,
                ContentType = document.ContentType,
                Type = document.Type,
                StudentFullName = document.StudentFullName,
                StudentId = document.StudentId
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id, document.FullName);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument
            {
                Id = entity.Id,
                FullName = entity.FullName
            };
    }    
}
