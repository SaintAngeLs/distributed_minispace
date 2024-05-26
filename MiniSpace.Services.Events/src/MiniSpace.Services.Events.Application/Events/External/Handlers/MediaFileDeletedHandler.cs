using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler: IEventHandler<MediaFileDeleted>
    {
        private readonly IEventRepository _eventRepository;
        
        public MediaFileDeletedHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            if(@event.Source.ToLowerInvariant() != "event")
            {
                return;
            }

            var foundEvent = await _eventRepository.GetAsync(@event.SourceId);
            if(foundEvent != null)
            {
                foundEvent.RemoveMediaFile(@event.MediaFileId);
                await _eventRepository.UpdateAsync(foundEvent);
            }
        }
    }
}