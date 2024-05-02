using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(Guid Id, Guid PostId, Guid StudentId, List<Guid> Likes,
        Guid ParentId, string Comment, DateTime PublishDate);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument()
            {
                Id = entity.Id;
                PostId = entity.PostId;
                StudentId = entity.StudentId;
                Likes = entity.Likes;
                ParentId = entity.ParentId;
                Comment = entity.Comment;
                PublishDate = entity.PublishDate;
            };

        public static CommentDto AsDto(this CommentDocument document)
            => new CommentDto()
            {
                Id = document.Id;
                PostId = document.PostId;
                StudentId = document.StudentId;
                Likes = document.Likes;
                ParentId = document.ParentId;
                Comment = document.Comment;
                PublishDate = document.PublishDate;
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
