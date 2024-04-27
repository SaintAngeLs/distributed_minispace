using Convey.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class ChangeStudentStateHandler : ICommandHandler<ChangeStudentState>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        
        public ChangeStudentStateHandler(IStudentRepository studentRepository, IEventMapper eventMapper,
            IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(ChangeStudentState command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var state))
            {
                throw new CannotChangeStudentStateException(student.Id, State.Unknown);
            }

            if (student.State == state)
            {
                throw new StudentStateAlreadySetException(student.Id, state);
            }

            switch (state)
            {
                case State.Incomplete:
                    student.SetIncomplete();
                    break;
                case State.Valid:
                    student.SetValid();
                    break;
                case State.Banned:
                    student.SetBanned();
                    break;
                default:
                    throw new CannotChangeStudentStateException(student.Id, state);
            }
            
            await _studentRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
