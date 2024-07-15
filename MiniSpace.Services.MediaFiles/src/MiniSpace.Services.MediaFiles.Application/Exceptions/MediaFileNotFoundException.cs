namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class MediaFileNotFoundException: AppException
    {
        public override string Code { get; } = "media_file_not_found";
        public string MediaFileUrl { get; }

        public MediaFileNotFoundException(string mediaFileUrl)
            : base($"Media file with ID: {mediaFileUrl} was not found.")
        {
            MediaFileUrl = mediaFileUrl;
        }
    }
}