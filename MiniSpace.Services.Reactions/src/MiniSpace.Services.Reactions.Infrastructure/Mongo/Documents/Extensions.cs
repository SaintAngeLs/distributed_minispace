using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Reaction AsEntity(this ReactionDocument document)
            => new Reaction(document.Id, document.StudentId, document.StudentFullName, document.Type,
                    document.ContentId, document.ContentType);

        public static ReactionDocument AsDocument(this Reaction entity)
            => new ReactionDocument()
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                StudentFullName = entity.StudentFullName,
                Type = entity.ReactionType,
                ContentId = entity.ContentId,
                ContentType = entity.ContentType,
                
            };

        public static ReactionDto AsDto(this ReactionDocument document)
            => new ReactionDto()
            {
                Id = document.Id,
                StudentId = document.StudentId,
                StudentFullName = document.StudentFullName,
                Type = document.Type,
                ContentId = document.ContentId,
                ContentType = document.ContentType
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument
            {
                Id = entity.Id
            };

            public static EventDocument AsDocument(this Event entity)
            => new EventDocument
            {
                Id = entity.Id
            };

            public static PostDocument AsDocument(this Post entity)
            => new PostDocument
            {
                Id = entity.Id
            };
    }    
}
