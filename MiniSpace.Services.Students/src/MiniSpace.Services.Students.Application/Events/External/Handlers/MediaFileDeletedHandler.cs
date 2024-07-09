using Convey.CQRS.Events;
using MiniSpace.Services.Students.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler : IEventHandler<MediaFileDeleted>
    {
        private readonly IStudentRepository _studentRepository;

        public MediaFileDeletedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            if (@event.Source.ToLowerInvariant() != "studentprofile")
            {
                return;
            }

            var student = await _studentRepository.GetAsync(@event.SourceId);
            if (student != null)
            {
                bool updated = false;

                // Check and remove profile image
                if (student.ProfileImageUrl == @event.MediaFileUrl)
                {
                    student.RemoveProfileImage();
                    updated = true;
                }

                // Check and remove banner image
                if (student.BannerUrl == @event.MediaFileUrl)
                {
                    student.RemoveBannerImage();
                    updated = true;
                }

                // Check and remove gallery images
                if (student.GalleryOfImageUrls.Contains(@event.MediaFileUrl))
                {
                    student.RemoveGalleryImage(@event.MediaFileUrl);
                    updated = true;
                }

                if (updated)
                {
                    await _studentRepository.UpdateAsync(student);
                }
            }
        }
    }
}
