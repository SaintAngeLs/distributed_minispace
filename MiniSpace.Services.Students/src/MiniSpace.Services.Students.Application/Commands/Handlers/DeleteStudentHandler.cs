using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
//using MiniSpace.Services.Students.Application.Events;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class DeleteStudentHandler : ICommandHandler<DeleteStudent>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        
        public DeleteStudentHandler(IStudentRepository studentRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _studentRepository = studentRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteStudent command, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetAsync(command.Id);
            if (student is null)
            {
                throw new StudentNotFoundException(command.Id);
            }

            var identity = _appContext.Identity;
            if (identity.Id != student.Id)
            {
                throw new UnauthorizedStudentAccessException(command.Id, identity.Id);
            }

            // await _studentRepository.DeleteAsync(command.Id);

            // await _messageBroker.PublishAsync(new StudentDeleted(command.Id));
        }
    }    
}
