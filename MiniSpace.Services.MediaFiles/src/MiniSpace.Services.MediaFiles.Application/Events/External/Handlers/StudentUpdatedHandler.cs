using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using System;
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
            var profileImageInfo = await _fileSourceInfoRepository.GetAsync(@event.MediaFileId);
            if (profileImageInfo != null)
            {
                profileImageInfo.Associate();
                await _fileSourceInfoRepository.UpdateAsync(profileImageInfo);
            }

            if (@event.BannerMediaFileId != Guid.Empty)
            {
                var bannerImageInfo = await _fileSourceInfoRepository.GetAsync(@event.BannerMediaFileId);
                if (bannerImageInfo != null)
                {
                    bannerImageInfo.Associate();
                    await _fileSourceInfoRepository.UpdateAsync(bannerImageInfo);
                }
            }

            var galleryImageInfos = await _fileSourceInfoRepository.FindAsync(@event.StudentId, ContextType.StudentProfile);
            foreach (var galleryImageInfo in galleryImageInfos)
            {
                if (@event.GalleryOfImages.Contains(galleryImageInfo.Id))
                {
                    galleryImageInfo.Associate();
                }
                else
                {
                    galleryImageInfo.Unassociate();
                }
                await _fileSourceInfoRepository.UpdateAsync(galleryImageInfo);
            }

            var allStudentFiles = await _fileSourceInfoRepository.FindAsync(@event.StudentId, ContextType.StudentProfile);
            foreach (var file in allStudentFiles)
            {
                if (file.Id != @event.MediaFileId && file.Id != @event.BannerMediaFileId && !@event.GalleryOfImages.Contains(file.Id))
                {
                    file.Unassociate();
                    await _fileSourceInfoRepository.UpdateAsync(file);
                }
            }
        }
    }
}
