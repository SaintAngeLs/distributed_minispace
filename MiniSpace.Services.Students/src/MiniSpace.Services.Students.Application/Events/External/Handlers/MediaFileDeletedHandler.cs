using Convey.CQRS.Events;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Linq;
using System.Text.Json;
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

            // Fetch the student data
            var student = await _studentRepository.GetAsync(@event.UploaderId);
            if (student == null)
            {
                Console.WriteLine($"Student with ID {@event.UploaderId} not found.");
                return;
            }

            // Fetch the user gallery using the student's ID
            var userGallery = await _userGalleryRepository.GetAsync(student.Id);
            Console.WriteLine($"Fetched student and user gallery data. {JsonSerializer.Serialize(userGallery, new JsonSerializerOptions { WriteIndented = true })}");

            bool studentUpdated = false;
            bool galleryUpdated = false;

            // Handle profile image deletion
            if (@event.Source.ToLowerInvariant() == "studentprofileimage" && student.ProfileImageUrl == @event.MediaFileUrl)
            {
                student.RemoveProfileImage();
                studentUpdated = true;
                Console.WriteLine("Removed profile image.");
            }

            // Handle banner image deletion
            if (@event.Source.ToLowerInvariant() == "studentbannerimage" && student.BannerUrl == @event.MediaFileUrl)
            {
                student.RemoveBannerImage();
                studentUpdated = true;
                Console.WriteLine("Removed banner image.");
            }

            // Handle gallery image deletion
            if (userGallery != null)
            {
                Console.WriteLine("User gallery is not null");
                var galleryImage = userGallery.GalleryOfImages.FirstOrDefault(img => img.ImageUrl == @event.MediaFileUrl);
                if (galleryImage != null)
                {
                    Console.WriteLine("Gallery image found is not null");
                    userGallery.RemoveGalleryImage(galleryImage.ImageId);
                    galleryUpdated = true;
                    Console.WriteLine("Removed gallery image.");
                }
            }

            // Update the student repository if necessary
            if (studentUpdated)
            {
                await _studentRepository.UpdateAsync(student);
                Console.WriteLine("Updated student repository.");
            }

            // Update the user gallery repository if necessary
            if (galleryUpdated)
            {
                await _userGalleryRepository.UpdateAsync(userGallery);
                Console.WriteLine("Updated user gallery repository.");
            }
        }
    }
}
