using Convey.CQRS.Events;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler : IEventHandler<MediaFileDeleted>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserGalleryRepository _userGalleryRepository;

        public MediaFileDeletedHandler(IStudentRepository studentRepository, IUserGalleryRepository userGalleryRepository)
        {
            _studentRepository = studentRepository;
            _userGalleryRepository = userGalleryRepository;
        }

        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received MediaFileDeleted event: {@event.MediaFileUrl}");

            var student = await _studentRepository.GetAsync(@event.SourceId);
            if (student != null)
            {
                bool updated = false;

                // Check and remove profile image
                if (@event.Source.ToLowerInvariant() == "studentprofileimage" && student.ProfileImageUrl == @event.MediaFileUrl)
                {
                    student.RemoveProfileImage();
                    updated = true;
                    Console.WriteLine("Removed profile image.");
                }

                // Check and remove banner image
                if (@event.Source.ToLowerInvariant() == "studentbannerimage" && student.BannerUrl == @event.MediaFileUrl)
                {
                    student.RemoveBannerImage();
                    updated = true;
                    Console.WriteLine("Removed banner image.");
                }

                if (updated)
                {
                    await _studentRepository.UpdateAsync(student);
                    Console.WriteLine("Updated student repository.");
                }
            }

            // Check and remove gallery images
            var userGallery = await _userGalleryRepository.GetAsync(@event.SourceId);
            if (userGallery != null)
            {
                var galleryImage = userGallery.GalleryOfImages.FirstOrDefault(img => img.ImageUrl == @event.MediaFileUrl);
                if (galleryImage != null)
                {
                    userGallery.RemoveGalleryImage(galleryImage.ImageId);
                    await _userGalleryRepository.UpdateAsync(userGallery);
                    Console.WriteLine("Removed gallery image.");
                }
            }
        }
    }
}
