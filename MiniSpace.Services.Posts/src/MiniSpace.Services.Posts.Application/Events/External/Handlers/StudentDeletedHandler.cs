using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Repositories;

namespace MiniSpace.Services.Posts.Application.Events.External.Handlers
{
    public class StudentDeletedHandler : IEventHandler<StudentDeleted>
    {
        private readonly IStudentRepository _studentRepository;

        public StudentDeletedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task HandleAsync(StudentDeleted @event, CancellationToken cancellationToken = default)
        {
            if (!(await _studentRepository.ExistsAsync(@event.StudentId)))
            {
                throw new StudentNotFoundException(@event.StudentId);
            }

            await _studentRepository.DeleteAsync(@event.StudentId);
        }
    }    
}
