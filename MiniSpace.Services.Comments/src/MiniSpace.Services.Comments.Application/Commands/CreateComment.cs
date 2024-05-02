using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class CreateComment : ICommand
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid StudentId { get; set; }
        public List<Guid> Likes { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
        public DateTime PublishDate { get; set; }
 

        public CreateComment(Guid id, Guid postId, Guid studentId, List<Guid> likes,
            Guid parentId, string comment, DateTime publishDate)
        {
            Id = id;
            PostId = postId;
            StudentId = studentId;
            Likes = likes;
            ParentId = parentId;
            Comment = comment;
            PublishDate = publishDate;
        }
    }
}