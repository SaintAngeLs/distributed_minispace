namespace MiniSpace.Services.Comments.Application.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid StudentId { get; set; }
        public List<Guid> Likes { get; set; }
        public Guid ParentId { get; set; }
        public string Comment { get; set; }
        public DateTime PublishDate { get; set; }
    }
}