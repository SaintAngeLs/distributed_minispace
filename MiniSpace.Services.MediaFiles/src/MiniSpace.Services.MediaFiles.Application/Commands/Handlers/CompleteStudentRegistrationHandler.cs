using Convey.CQRS.Commands;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Exceptions;
using MiniSpace.Services.MediaFiles.Core.Repositories;

namespace MiniSpace.Services.MediaFiles.Application.Commands.Handlers
{
    public class CompleteStudentRegistrationHandler : ICommandHandler<CompleteStudentRegistration>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public CompleteStudentRegistrationHandler(IStudentRepository studentRepository, 
            IDateTimeProvider dateTimeProvider, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _dateTimeProvider = dateTimeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(CompleteStudentRegistration command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (student.State is Core.Entities.State.Valid)
            {
                throw new StudentAlreadyRegisteredException(command.StudentId);
            }
            
            student.CompleteRegistration(command.ProfileImage, command.Description,
                command.DateOfBirth, _dateTimeProvider.Now, command.EmailNotifications);
            await _studentRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
