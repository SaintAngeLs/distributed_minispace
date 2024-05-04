using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Core.Repositories;

namespace MiniSpace.Services.Comments.Application.Events.External.Handlers
{
    public class EventDeletedHandler : IEventHandler<EventDeleted>
    {
        private readonly ICommentRepository _commentRepository;

        public EventDeletedHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        
        public async Task HandleAsync(EventDeleted @event, CancellationToken cancellationToken = default)
        {
            //if (!(await _eventRepository.ExistsAsync(@event.EventId)))
            //{
            //    throw new EventNotFoundException(@event.EventId);
            //}

            //await _eventRepository.DeleteAsync(@event.EventId);
        }
    }    
}
