using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Events.External.Handlers
{
    public class EventImageUploadedHandler : IEventHandler<EventImageUploaded>
    {
        private readonly IEventRepository _eventRepository;

        public EventImageUploadedHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task HandleAsync(EventImageUploaded @event, CancellationToken cancellationToken)
        {
            // Retrieve the event from the repository
            var foundEvent = await _eventRepository.GetAsync(@event.EventId);
            if (foundEvent == null)
            {
                // If the event is not found, return early
                return;
            }

            // Update the event based on the type of image uploaded
            if (@event.ImageType.ToLowerInvariant() == "eventbanner")
            {
                // Set the banner URL for the event
                foundEvent.UpdateBannerUrl(@event.ImageUrl);
            }
            else if (@event.ImageType.ToLowerInvariant() == "eventgalleryimage")
            {
                // Add the image to the gallery
                foundEvent.AddGalleryImage(@event.ImageUrl);
            }

            // Save the updated event back to the repository
            await _eventRepository.UpdateAsync(foundEvent);
        }
    }
}
