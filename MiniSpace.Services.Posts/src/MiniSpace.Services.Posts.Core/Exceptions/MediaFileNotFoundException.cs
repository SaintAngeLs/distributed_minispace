namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class MediaFileNotFoundException : DomainException
    {
        public override string Code { get; } = "media_file_not_found";
        public string MediaFileUrl { get; }
        public Guid PostId { get; }

        public MediaFileNotFoundException(string mediaFileUrl, Guid postId) 
            : base($"Media file with ID: '{mediaFileUrl}' for post with ID: {postId} was not found.")
        {
            MediaFileUrl = mediaFileUrl;
            PostId = postId;
        }
    }
}