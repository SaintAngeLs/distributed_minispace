namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class MediaFileNotFoundException: AppException
    {
        public override string Code { get; } = "media_file_not_found";
        public Guid MediaFileId { get; }

        public MediaFileNotFoundException(Guid mediaFileId)
            : base($"Media file with ID: {mediaFileId} was not found.")
        {
            MediaFileId = mediaFileId;
        }
    }
}