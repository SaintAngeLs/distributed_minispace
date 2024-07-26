using Convey.CQRS.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class StudentImageUploadedHandler : IEventHandler<StudentImageUploaded>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserGalleryRepository _userGalleryRepository;

        public StudentImageUploadedHandler(IStudentRepository studentRepository, IUserGalleryRepository userGalleryRepository)
        {
            _studentRepository = studentRepository;
            _userGalleryRepository = userGalleryRepository;
        }

        public async Task HandleAsync(StudentImageUploaded @event, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetAsync(@event.StudentId);
            if (student == null)
            {
                throw new StudentNotFoundException(@event.StudentId);
            }

            switch (@event.ImageType)
            {
                case nameof(ContextType.StudentProfileImage):
                    student.UpdateProfileImageUrl(@event.ImageUrl);
                    Console.WriteLine("Updated profile image URL.");
                    break;

                case nameof(ContextType.StudentBannerImage):
                    student.UpdateBannerUrl(@event.ImageUrl);
                    break;

                case nameof(ContextType.StudentGalleryImage):
                    var userGallery = await _userGalleryRepository.GetAsync(@event.StudentId);
                    if (userGallery == null)
                    {
                        userGallery = new UserGallery(@event.StudentId);
                    }
                    userGallery.AddGalleryImage(Guid.NewGuid(), @event.ImageUrl);
                    await _userGalleryRepository.UpdateAsync(userGallery);
                    break;

                default:
                    throw new InvalidContextTypeException(@event.ImageType);
            }

            await _studentRepository.UpdateAsync(student);
        }
    }
}
