using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserGalleryDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<GalleryImageDto> GalleryOfImages { get; set; }

        public UserGalleryDto(Guid userId, IEnumerable<GalleryImageDto> galleryOfImages)
        {
            UserId = userId;
            GalleryOfImages = galleryOfImages ?? throw new ArgumentNullException(nameof(galleryOfImages));
        }
    }
}
