using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Exceptions;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class CompleteStudentRegistrationHandler : ICommandHandler<CompleteUserRegistration>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public CompleteStudentRegistrationHandler(IUserRepository userRepository, 
            IDateTimeProvider dateTimeProvider, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(CompleteUserRegistration command, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(command.StudentId);
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
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
