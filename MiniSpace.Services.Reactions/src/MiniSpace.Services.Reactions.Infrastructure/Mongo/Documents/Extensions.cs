using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Reaction AsEntity(this ReactionDocument document)
            => new Reaction(document.StudentId, document.Type, document.StudentFullName,
                    document.ContentType, document.ContentId);

        public static ReactionDocument AsDocument(this Reaction entity)
            => new ReactionDocument()
            {
                Id = entity.Id,
                ContentId = entity.ContentId,
                ContentType = entity.ContentType,
                Type = entity.ReactionType,
                StudentId = entity.StudentId,
                StudentFullName = entity.StudentFullName
            };

        public static ReactionDto AsDto(this ReactionDocument document)
            => new ReactionDto()
            {
                Id = document.Id,
                ContentId = document.ContentId,
                ContentType = document.ContentType,
                Type = document.Type,
                StudentId = document.StudentId,
                StudentFullName = document.StudentFullName
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument
            {
                Id = entity.Id
            };
    }    
}
