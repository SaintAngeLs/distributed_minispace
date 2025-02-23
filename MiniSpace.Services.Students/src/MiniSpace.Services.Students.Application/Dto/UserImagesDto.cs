using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserImagesDto
    {
        public Guid UserId { get; set; }
        public string BannerUrl { get; set; }
        public IEnumerable<string> GalleryOfImageUrls { get; set; }

        public UserImagesDto(Guid userId, string bannerMediaFileId, IEnumerable<string> galleryOfImages)
        {
            UserId = userId;
            BannerUrl = bannerMediaFileId;
            GalleryOfImageUrls = galleryOfImages ?? new List<string>();
        }
    }
}
