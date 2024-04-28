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

        public Comment(Guid Id, Guid PostId, Guid StudentId, List<Guid> Likes,
        Guid ParentId, string Comment, DateTime PublishDate)
        {
            Id = Id;
            PostId = PostId;
            StudentId = StudentId;
            Likes = Likes;
            ParentId = ParentId;
            Comment = Comment;
            PublishDate = PublishDate;
        }
    }
}