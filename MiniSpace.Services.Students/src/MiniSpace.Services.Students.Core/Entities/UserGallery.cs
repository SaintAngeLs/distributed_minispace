using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserGallery
    {
        public Guid UserId { get; private set; }
        private ISet<GalleryImage> _galleryOfImages = new HashSet<GalleryImage>();

        public IEnumerable<GalleryImage> GalleryOfImages
        {
            get => _galleryOfImages;
            private set => _galleryOfImages = new HashSet<GalleryImage>(value ?? Enumerable.Empty<GalleryImage>());
        }

        public UserGallery(Guid userId)
        {
            UserId = userId;
        }

        public void AddGalleryImage(Guid imageId, string imageUrl)
        {
            _galleryOfImages.Add(new GalleryImage(imageId, imageUrl, DateTime.UtcNow));
        }

        public void UpdateGalleryOfImages(IEnumerable<GalleryImage> galleryOfImages)
        {
            GalleryOfImages = new HashSet<GalleryImage>(galleryOfImages ?? Enumerable.Empty<GalleryImage>());
        }

        public void RemoveGalleryImage(Guid imageId)
        {
            var image = _galleryOfImages.FirstOrDefault(img => img.ImageId == imageId);
            if (image == null)
            {
                throw new StudentGalleryImageNotFoundException(UserId, imageId.ToString());
            }

            _galleryOfImages.Remove(image);
        }
    }
}
