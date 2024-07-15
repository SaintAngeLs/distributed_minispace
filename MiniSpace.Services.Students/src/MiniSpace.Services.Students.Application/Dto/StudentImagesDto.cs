using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class StudentImagesDto
    {
        public Guid StudentId { get; set; }
        public string BannerUrl { get; set; }
        public IEnumerable<string> GalleryOfImageUrls { get; set; }

        public StudentImagesDto(Guid studentId, string bannerMediaFileId, IEnumerable<string> galleryOfImages)
        {
            StudentId = studentId;
            BannerUrl = bannerMediaFileId;
            GalleryOfImageUrls = galleryOfImages ?? new List<string>();
        }
    }
}
