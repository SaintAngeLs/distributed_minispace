namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class UserGalleryImageNotFoundException : DomainException
    {
        public override string Code { get; } = "student_gallery_image_not_found";
        public Guid UserId { get; }
        public string MediaFileId { get; }

        public UserGalleryImageNotFoundException(Guid userId, string mediaFileId) 
            : base($"Student with id: {userId} does not have an image with media file id: {mediaFileId} in the gallery.")
        {
            UserId = userId;
            MediaFileId = mediaFileId;
        }
    }
}
