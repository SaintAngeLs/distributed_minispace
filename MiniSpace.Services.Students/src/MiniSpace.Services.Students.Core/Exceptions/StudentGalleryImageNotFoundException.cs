namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentGalleryImageNotFoundException : DomainException
    {
        public override string Code { get; } = "student_gallery_image_not_found";
        public Guid StudentId { get; }
        public string MediaFileId { get; }

        public StudentGalleryImageNotFoundException(Guid studentId, string mediaFileId) 
            : base($"Student with id: {studentId} does not have an image with media file id: {mediaFileId} in the gallery.")
        {
            StudentId = studentId;
            MediaFileId = mediaFileId;
        }
    }
}
