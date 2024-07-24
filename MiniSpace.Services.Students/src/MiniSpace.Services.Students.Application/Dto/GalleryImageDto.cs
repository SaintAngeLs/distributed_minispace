using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    public class GalleryImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; }

        public GalleryImageDto(Guid imageId, string imageUrl)
        {
            ImageId = imageId;
            ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        }
    }
}