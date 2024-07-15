using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Application.Events.External.Handlers
{
    public class StudentUpdatedHandler : IEventHandler<StudentUpdated>
    {
        private readonly IFileSourceInfoRepository _fileSourceInfoRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public StudentUpdatedHandler(IFileSourceInfoRepository fileSourceInfoRepository, ICommandDispatcher commandDispatcher)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(StudentUpdated @event, CancellationToken cancellationToken = default)
        {
            // Handle profile image
            // var profileImageInfo = await _fileSourceInfoRepository.GetAsync(@event.ProfileImageUrl);
            // if (profileImageInfo != null)
            // {
            //     profileImageInfo.Associate();
            //     await _fileSourceInfoRepository.UpdateAsync(profileImageInfo);
            // }

            // Handle banner image
            // if (!string.IsNullOrEmpty(@event.BannerUrl))
            // {
            //     var bannerImageInfo = await _fileSourceInfoRepository.GetAsync(@event.BannerUrl);
            //     if (bannerImageInfo != null)
            //     {
            //         bannerImageInfo.Associate();
            //         await _fileSourceInfoRepository.UpdateAsync(bannerImageInfo);
            //     }
            // }

            // Handle gallery images
            // foreach (var galleryImageUrl in @event.GalleryOfImageUrls)
            // {
            //     var galleryImageInfo = await _fileSourceInfoRepository.GetAsync(galleryImageUrl);
            //     if (galleryImageInfo != null)
            //     {
            //         galleryImageInfo.Associate();
            //         await _fileSourceInfoRepository.UpdateAsync(galleryImageInfo);
            //     }
            // }

            // Unassociate files that are no longer associated with the student
            // var allStudentFiles = await _fileSourceInfoRepository.FindAsync(@event.StudentId, ContextType.StudentProfileImage);
            // foreach (var file in allStudentFiles)
            // {
            //     if (file.FileUrl != @event.ProfileImageUrl && file.FileUrl != @event.BannerUrl && !@event.GalleryOfImageUrls.Contains(file.FileUrl))
            //     {
            //         file.Unassociate();
            //         await _fileSourceInfoRepository.UpdateAsync(file);
            //     }
            // }
        }
    }
}
