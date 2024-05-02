namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment{
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Guid StudentId { get; private set; }
        public List<Guid> Likes { get; private set; }
        public Guid ParentId { get; private set; }
        public string Comment { get; private set; }
        public DateTime PublishDate { get; private set; }

        public Comment(Guid id, Guid postId, Guid studentId, List<Guid> likes,
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