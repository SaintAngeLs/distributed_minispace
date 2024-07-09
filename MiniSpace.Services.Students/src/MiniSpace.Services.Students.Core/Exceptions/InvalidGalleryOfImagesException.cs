namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidGalleryOfImagesException : DomainException
    {
        public override string Code { get; } = "invalid_gallery_of_images";
        public Guid Id { get; }

        public InvalidGalleryOfImagesException(Guid id) : base($"Student with id: {id} has an invalid gallery of images.")
        {
            Id = id;
        }
    }
}