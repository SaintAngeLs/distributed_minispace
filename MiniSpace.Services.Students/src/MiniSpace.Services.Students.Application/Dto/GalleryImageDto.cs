using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class GalleryImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        public GalleryImageDto(Guid imageId, string imageUrl, DateTime dateAdded)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
            DateAdded = dateAdded;
        }
    }
}
