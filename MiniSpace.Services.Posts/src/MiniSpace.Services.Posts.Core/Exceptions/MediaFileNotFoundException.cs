namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class MediaFileNotFoundException : DomainException
    {
        public override string Code { get; } = "media_file_not_found";
        public Guid MediaFileId { get; }
        public Guid PostId { get; }

        public MediaFileNotFoundException(Guid mediaFileId, Guid postId) 
            : base($"Media file with ID: '{mediaFileId}' for post with ID: {postId} was not found.")
        {
            MediaFileId = mediaFileId;
            PostId = postId;
        }
    }
}