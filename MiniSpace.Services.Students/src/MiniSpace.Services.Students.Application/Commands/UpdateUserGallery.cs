using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateUserGallery : ICommand
    {
        public Guid UserId { get; }
        public IEnumerable<GalleryImageDto> GalleryOfImages { get; }

        public UpdateUserGallery(Guid userId, IEnumerable<GalleryImageDto> galleryOfImages)
        {
            UserId = userId;
            GalleryOfImages = galleryOfImages ?? throw new ArgumentNullException(nameof(galleryOfImages));
        }
    }
}