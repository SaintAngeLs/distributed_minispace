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

        public StudentImageUploadedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
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
                    break;
                case nameof(ContextType.StudentBannerImage):
                    student.UpdateBannerUrl(@event.ImageUrl);
                    break;
                case nameof(ContextType.StudentGalleryImage):
                    student.AddGalleryImageUrl(@event.ImageUrl);
                    break;
                default:
                    throw new InvalidContextTypeException(@event.ImageType);
            }

            await _studentRepository.UpdateAsync(student);
        }
    }
}
