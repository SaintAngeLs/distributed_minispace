namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class UnauthorizedMediaFileAccessException: AppException
    {
        public override string Code { get; } = "unauthorized_media_file_access";
        public Guid MediaFileId { get; }
        public Guid UserId { get; }
        public Guid UploaderId { get; }

        public UnauthorizedMediaFileAccessException(Guid mediaFileId, Guid userId, Guid uploaderId)
            : base($"User with ID: {userId} tried to access media file with ID: {mediaFileId} uploaded by user with ID: {uploaderId}.")
        {
            MediaFileId = mediaFileId;
            UserId = userId;
            UploaderId = uploaderId;
        }
    }
}