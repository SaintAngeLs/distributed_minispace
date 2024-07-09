using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateStudentHandler : ICommandHandler<UpdateStudent>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateStudentHandler(IStudentRepository studentRepository, IAppContext appContext,
            IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateStudent command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != student.Id && !identity.IsAdmin)
            {
                throw new UnauthorizedStudentAccessException(command.StudentId, identity.Id);
            }

            student.Update(command.ProfileImageUrl, command.Description, command.EmailNotifications);
            student.UpdateBannerUrl(command.BannerUrl);
            student.UpdateGalleryOfImageUrls(command.GalleryOfImageUrls);
            student.UpdateEducation(command.Education);
            student.UpdateWorkPosition(command.WorkPosition);
            student.UpdateCompany(command.Company);
            student.UpdateLanguages(command.Languages);
            student.UpdateInterests(command.Interests);

            if (command.EnableTwoFactor)
            {
                student.EnableTwoFactorAuthentication(command.TwoFactorSecret);
            }

            if (command.DisableTwoFactor)
            {
                student.DisableTwoFactorAuthentication();
            }

            await _studentRepository.UpdateAsync(student);

            var studentUpdatedEvent = new StudentUpdated(
                student.Id,
                student.FullName,
                student.ProfileImageUrl,
                student.BannerUrl,
                student.GalleryOfImageUrls,
                student.Education,
                student.WorkPosition,
                student.Company,
                student.Languages,
                student.Interests
            );

            await _messageBroker.PublishAsync(studentUpdatedEvent);
        }
    }
}
