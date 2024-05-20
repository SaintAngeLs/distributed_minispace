using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Application.Dto
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid OrganizerId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
        public string State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public PostDto()
        {
        }

        public PostDto(Post post)
        {
            Id = post.Id;
            EventId = post.EventId;
            OrganizerId = post.OrganizerId;
            TextContent = post.TextContent;
            MediaFiles = post.MediaFiles;
            State = post.State.ToString();
            PublishDate = post.PublishDate;
            CreatedAt = post.CreatedAt;
            UpdatedAt = post.UpdatedAt;
        }
    }
}
