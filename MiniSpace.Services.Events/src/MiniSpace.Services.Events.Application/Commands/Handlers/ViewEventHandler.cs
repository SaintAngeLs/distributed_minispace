using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class ViewEventHandler : ICommandHandler<ViewEvent>
    {
        private readonly IEventsUserViewsRepository _eventsUserViewsRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<ViewEventHandler> _logger;

        public ViewEventHandler(
            IEventsUserViewsRepository eventsUserViewsRepository,
            IEventRepository eventRepository,
            ILogger<ViewEventHandler> logger)
        {
            _eventsUserViewsRepository = eventsUserViewsRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ViewEvent command, CancellationToken cancellationToken)
        {
            // Ensure the event exists
            var eventExists = await _eventRepository.ExistsAsync(command.EventId);
            if (!eventExists)
            {
                _logger.LogWarning($"Event with ID {command.EventId} not found.");
                return;
            }

            // Fetch the user's event views
            var userViews = await _eventsUserViewsRepository.GetAsync(command.UserId);
            if (userViews == null)
            {
                // If no views exist, create a new EventsViews object for the user
                userViews = new EventsViews(command.UserId, Enumerable.Empty<View>());
            }

            // Check if the event has already been viewed
            var existingView = userViews.Views.FirstOrDefault(v => v.EventId == command.EventId);
            if (existingView != null)
            {
                // Remove the existing view (to update the date)
                userViews.RemoveView(command.EventId);
            }

            // Add the new view with the current date
            userViews.AddView(command.EventId, DateTime.UtcNow);

            // Save the updated views to the repository
            await _eventsUserViewsRepository.UpdateAsync(userViews);

            _logger.LogInformation($"User {command.UserId} viewed event {command.EventId}.");
        }
    }
}
