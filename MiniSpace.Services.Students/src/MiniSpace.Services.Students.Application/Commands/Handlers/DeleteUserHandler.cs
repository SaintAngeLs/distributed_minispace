using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class DeleteUserHandler : ICommandHandler<DeleteUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeleteUserHandler(IUserRepository userRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteUser command, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(command.UserId);
            if (student is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != student.Id && !identity.IsAdmin)
            {
                throw new AnauthorizedUserAccessException(command.UserId, identity.Id);
            }

            await _userRepository.DeleteAsync(command.UserId);

            await _messageBroker.PublishAsync(new UserDeleted(command.UserId, student.FullName));
        }
    }    
}
