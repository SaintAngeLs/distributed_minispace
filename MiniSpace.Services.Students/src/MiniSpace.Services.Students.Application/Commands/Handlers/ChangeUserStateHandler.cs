using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class ChangeUserStateHandler : ICommandHandler<ChangeUserState>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        
        public ChangeUserStateHandler(IUserRepository userRepository, IEventMapper eventMapper,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(ChangeUserState command, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new UserNotFoundException(command.StudentId);
            }

            if (!Enum.TryParse<State>(command.State, true, out var state))
            {
                throw new CannotChangeUserStateException(student.Id, State.Unknown);
            }

            if (student.State == state)
            {
                throw new UserStateAlreadySetException(student.Id, state);
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
                case State.Unverified:
                    student.SetUnverified();
                    break;
                default:
                    throw new CannotChangeUserStateException(student.Id, state);
            }
            
            await _userRepository.UpdateAsync(student);
            
            var events = _eventMapper.MapAll(student.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }    
}
