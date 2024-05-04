using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Events.External.Handlers
{
    public class PostDeletedHandler : IEventHandler<PostDeleted>
    {
        private readonly ICommentRepository _commentRepository;

        public PostDeletedHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        
        public async Task HandleAsync(PostDeleted @event, CancellationToken cancellationToken = default)
        {
            //if (!(await _studentRepository.ExistsAsync(@event.StudentId)))
            //{
            //    throw new StudentNotFoundException(@event.StudentId);
            //}

            //await _studentRepository.DeleteAsync(@event.StudentId);
        }
    }    
}
