namespace MiniSpace.Services.Posts.Application.Dto
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
        public string TextContent { get; set; }
        public string MediaContent { get; set; }
    }
}
