using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class UpdateEventHandler : ICommandHandler<UpdateEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        public UpdateEventHandler(IEventRepository eventRepository, IEventValidator eventValidator, 
            IAppContext appContext, IMessageBroker messageBroker, IDateTimeProvider dateTimeProvider)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task HandleAsync(UpdateEvent command, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetAsync(command.EventId);
            if (@event is null)
            {
                throw new EventNotFoundException(command.EventId);
            }
            
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && !@event.IsOrganizer(identity.Id) && !identity.IsAdmin)
            {
                throw new UnauthorizedEventAccessException(@event.Id, identity.Id);
            }
            
            // Validate and update event properties
            _eventValidator.ValidateName(command.Name);
            _eventValidator.ValidateDescription(command.Description);
            var startDate = _eventValidator.ParseDate(command.StartDate, "event_start_date");
            var endDate = _eventValidator.ParseDate(command.EndDate, "event_end_date");
            var now = _dateTimeProvider.Now;
            _eventValidator.ValidateDates(now, startDate, "now", "event_start_date");
            _eventValidator.ValidateDates(startDate, endDate, "event_start_date", "event_end_date");
            
            var address = @event.Location.Update(command.BuildingName, command.Street, command.BuildingNumber, 
                command.ApartmentNumber, command.City, command.ZipCode, command.Country);
            _eventValidator.ValidateCapacity(command.Capacity);
            _eventValidator.ValidateFee(command.Fee);
            var category = _eventValidator.ParseCategory(command.Category);
            
            var publishDate = @event.PublishDate;
            var state = @event.State;
            if (command.PublishDate != string.Empty)
            {
                publishDate = _eventValidator.ParseDate(command.PublishDate, "event_publish_date");
                _eventValidator.ValidateDates(now, publishDate, "now", "event_publish_date");
                _eventValidator.ValidateDates(publishDate, startDate, "event_publish_date", "event_start_date");
                state = State.ToBePublished;
            }
            
            @event.Update(
                command.Name, 
                command.Description, 
                startDate, 
                endDate, 
                address, 
                command.MediaFilesUrl.ToList(), 
                command.BannerUrl, 
                command.Capacity, 
                command.Fee, 
                category, 
                state, 
                publishDate, 
                now, 
                command.Visibility, 
                command.Settings
            );

            await _eventRepository.UpdateAsync(@event);
            await _messageBroker.PublishAsync(new EventUpdated(
                @event.Id, 
                _dateTimeProvider.Now, 
                identity.Id, 
                @event.OrganizerType, 
                @event.MediaFiles)
            );
        }
    }
}
