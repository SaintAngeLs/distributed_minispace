namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class UnauthorizedMediaFileUploadException : AppException
    {
        public override string Code { get; } = "unauthorized_media_file_upload";
        public Guid IdentityId { get; }
        public Guid UploaderId { get; }

        public UnauthorizedMediaFileUploadException(Guid identityId, Guid uploaderId) 
            : base($"User with ID: {uploaderId} is not authorized to upload media files. Identity ID: {identityId}.")
        {
            IdentityId = identityId;
            UploaderId = uploaderId;
        }
    }
}