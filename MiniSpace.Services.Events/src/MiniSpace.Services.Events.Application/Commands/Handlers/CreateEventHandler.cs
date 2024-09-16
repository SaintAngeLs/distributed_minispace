using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Commands.Handlers
{
    public class CreateEventHandler : ICommandHandler<CreateEvent>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IOrganizationsServiceClient _organizationsServiceClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventValidator _eventValidator;
        private readonly IAppContext _appContext;

        public CreateEventHandler(IEventRepository eventRepository, IMessageBroker messageBroker,
            IOrganizationsServiceClient organizationsServiceClient, IDateTimeProvider dateTimeProvider,
            IEventValidator eventValidator, IAppContext appContext)
        {
            _eventRepository = eventRepository;
            _messageBroker = messageBroker;
            _organizationsServiceClient = organizationsServiceClient;
            _dateTimeProvider = dateTimeProvider;
            _eventValidator = eventValidator;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateEvent command, CancellationToken cancellationToken)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var commandJson = JsonSerializer.Serialize(command, options);
            var identity = _appContext.Identity;

            // Validate Event ID
            if (command.EventId == Guid.Empty || await _eventRepository.ExistsAsync(command.EventId))
            {
                throw new InvalidEventIdException(command.EventId);
            }

            // Validate Organizer Type
            if (!Enum.TryParse<OrganizerType>(command.OrganizerType, true, out var organizerType))
            {
                throw new ArgumentException($"Invalid OrganizerType value: {command.OrganizerType}");
            }

            // Validate Visibility
            if (!Enum.TryParse<Visibility>(command.Visibility, true, out var visibility))
            {
                throw new ArgumentException($"Invalid Visibility value: {command.Visibility}");
            }

            PaymentMethod? paymentMethod = null;
            if (command.Settings != null && command.Settings.RequiresPayment && !string.IsNullOrWhiteSpace(command.Settings.PaymentMethod))
            {
                if (!Enum.TryParse<PaymentMethod>(command.Settings.PaymentMethod, true, out var parsedPaymentMethod))
                {
                    throw new ArgumentException($"Invalid PaymentMethod value: {command.Settings.PaymentMethod}");
                }
                paymentMethod = parsedPaymentMethod;
            }

            _eventValidator.ValidateName(command.Name);
            _eventValidator.ValidateDescription(command.Description);
            var startDate = _eventValidator.ParseDate(command.StartDate, "event_start_date");
            var endDate = _eventValidator.ParseDate(command.EndDate, "event_end_date");
            var now = _dateTimeProvider.Now;
            _eventValidator.ValidateDates(now, startDate, "now", "event_start_date");
            _eventValidator.ValidateDates(startDate, endDate, "event_start_date", "event_end_date");

            // Create Address object
            var address = new Address(command.BuildingName, command.Street, command.BuildingNumber, command.ApartmentNumber, command.City, command.ZipCode, command.Country);
            
            // Validate Capacity and Fee
            _eventValidator.ValidateCapacity(command.Capacity);
            _eventValidator.ValidateFee(command.Fee);

            // Parse and Validate Category
            var category = _eventValidator.ParseCategory(command.Category);

            // Determine Publish Date and State
            var publishDate = now;
            var state = State.Published;
            if (!string.IsNullOrEmpty(command.PublishDate))
            {
                publishDate = _eventValidator.ParseDate(command.PublishDate, "event_publish_date");
                _eventValidator.ValidateDates(now, publishDate, "now", "event_publish_date");
                _eventValidator.ValidateDates(publishDate, startDate, "event_publish_date", "event_start_date");
                state = State.ToBePublished;
            }

            // Determine Organizer
            Organizer organizer;
            if (organizerType == OrganizerType.Organization)
            {
                if (command.OrganizationId == null)
                {
                    throw new ArgumentNullException(nameof(command.OrganizationId), "OrganizationId cannot be null for Organization-type events.");
                }

                var organization = await _organizationsServiceClient.GetAsync(command.OrganizationId.Value);
                if (organization == null)
                {
                    throw new OrganizationNotFoundException(command.OrganizationId.Value);
                }

                organizer = new Organizer(command.OrganizationId.Value, OrganizerType.Organization, userId: command.OrganizerId, organizationId: command.OrganizationId.Value);
            }
            else
            {
                organizer = new Organizer(command.OrganizerId, OrganizerType.User, userId: command.OrganizerId);
            }

            var settings = command.Settings != null
                ? new EventSettings
                {
                    RequiresApproval = command.Settings.RequiresApproval,
                    IsOnlineEvent = command.Settings.IsOnlineEvent,
                    IsPrivate = command.Settings.IsPrivate,
                    RequiresRSVP = command.Settings.RequiresRSVP,
                    AllowsGuests = command.Settings.AllowsGuests,
                    ShowAttendeesPublicly = command.Settings.ShowAttendeesPublicly,
                    SendReminders = command.Settings.SendReminders,
                    ReminderDaysBefore = command.Settings.ReminderDaysBefore,
                    EnableChat = command.Settings.EnableChat,
                    AllowComments = command.Settings.AllowComments,
                    RequiresPayment = command.Settings.RequiresPayment,
                    PaymentMethod = paymentMethod ?? PaymentMethod.Offline, 
                    PaymentReceiverDetails = command.Settings.PaymentReceiverDetails,
                    PaymentGateway = command.Settings.PaymentGateway,
                    IssueTickets = command.Settings.IssueTickets,
                    MaxTicketsPerPerson = command.Settings.MaxTicketsPerPerson,
                    TicketPrice = command.Settings.TicketPrice,
                    RecordEvent = command.Settings.RecordEvent,
                    CustomTermsAndConditions = command.Settings.CustomTermsAndConditions,
                    CustomFields = command.Settings.CustomFields
                }
                : null;

            var @event = Event.Create(
                command.EventId,
                command.Name,
                command.Description,
                organizer,
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
                visibility,
                settings);

            await _eventRepository.AddAsync(@event);
            await _messageBroker.PublishAsync(new EventCreated(
                @event.Id,
                @event.Organizer.OrganizerType,
                @event.Organizer.Id,
                @event.MediaFiles));
        }
    }
}
