using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateStudentHandler : ICommandHandler<UpdateStudent>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public UpdateStudentHandler(IStudentRepository studentRepository, IEventMapper eventMapper,
            IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
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
            
            student.Update(command.ProfileImage, command.Description, command.EmailNotifications);
            await _studentRepository.UpdateAsync(student);

            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
