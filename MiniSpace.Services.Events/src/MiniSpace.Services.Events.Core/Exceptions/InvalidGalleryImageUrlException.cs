using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class InvalidGalleryImageUrlException : DomainException
    {
        public override string Code { get; } = "invalid_gallery_image_url";

        public InvalidGalleryImageUrlException(string imageUrl)
            : base($"The gallery image URL '{imageUrl}' is invalid.")
        {
        }
    }
}