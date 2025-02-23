using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class DeleteStudentHandler : ICommandHandler<DeleteUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeleteStudentHandler(IUserRepository userRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteUser command, CancellationToken cancellationToken = default)
        {
            var student = await _userRepository.GetAsync(command.StudentId);
            if (student is null)
            {
                throw new StudentNotFoundException(command.StudentId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != student.Id && !identity.IsAdmin)
            {
                throw new UnauthorizedStudentAccessException(command.StudentId, identity.Id);
            }

            await _userRepository.DeleteAsync(command.StudentId);

            await _messageBroker.PublishAsync(new StudentDeleted(command.StudentId, student.FullName));
        }
    }    
}
