using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class StudentImagesDto
    {
        public Guid StudentId { get; set; }
        public Guid BannerMediaFileId { get; set; }
        public IEnumerable<Guid> GalleryOfImages { get; set; }

        public StudentImagesDto(Guid studentId, Guid bannerMediaFileId, IEnumerable<Guid> galleryOfImages)
        {
            StudentId = studentId;
            BannerMediaFileId = bannerMediaFileId;
            GalleryOfImages = galleryOfImages ?? new List<Guid>();
        }
    }
}
