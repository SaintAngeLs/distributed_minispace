using FluentAssertions;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Events.Application.UnitTests.Commands.Handlers
{
    public class CreateEventHandlerTest
    {
        private readonly CreateEventHandler _createEventHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IOrganizationsServiceClient> _rganizationsServiceClientMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly Mock<IEventValidator> _eventValidatorMock;
        private readonly Mock<IAppContext> _appContextMock;

        public CreateEventHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _rganizationsServiceClientMock = new Mock<IOrganizationsServiceClient>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _eventValidatorMock = new Mock<IEventValidator>();
            _appContextMock = new Mock<IAppContext>();
            _createEventHandler = new CreateEventHandler(
                _eventRepositoryMock.Object,
                _messageBrokerMock.Object,
                _rganizationsServiceClientMock.Object,
                _dateTimeProviderMock.Object,
                _eventValidatorMock.Object,
                _appContextMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", string.Empty);

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "Organizer", true, new Dictionary<string, string>());
            _eventValidatorMock.Setup(val => val.ParseDate(command.PublishDate, "event_publish_date")).Returns(new DateTime(2024, 10, 1));
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { command.OrganizerId };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync(organization);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_IsNotOrganizerRole_ShouldThrowAuthorizedUserIsNotAnOrganizerException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", new DateTime(2024, 10, 1).ToString());

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { command.OrganizerId };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync(organization);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<AuthorizedUserIsNotAnOrganizerException>();
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_IsNotOrganizer_ShouldThrowOrganizerCannotAddEventForAnotherOrganizerException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", new DateTime(2024, 10, 1).ToString());

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "Organizer", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { command.OrganizerId };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync(organization);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<OrganizerCannotAddEventForAnotherOrganizerException>();
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_AlreadyExistingEvent_ShouldThrowInvalidEventIdException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", new DateTime(2024, 10, 1).ToString());

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "Organizer", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { command.OrganizerId };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync(organization);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<InvalidEventIdException>();
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_NonExistingOrganization_ShouldThrowOrganizationNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", new DateTime(2024, 10, 1).ToString());

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "Organizer", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { command.OrganizerId };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync((DTO.OrganizationDto)null);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<OrganizationNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_NonExistingEvent_ShouldThrowOrganizerDoesNotBelongToOrganizationException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new CreateEvent(eventId, "name", organizerId, Guid.NewGuid(),
                Guid.NewGuid(), new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", new DateTime(2024, 10, 1).ToString());

            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "Organizer", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            var organization = new DTO.OrganizationDto();
            organization.Id = command.OrganizationId;
            organization.Name = "name";
            organization.Organizers = new List<Guid> { };
            _rganizationsServiceClientMock.Setup(rg => rg.GetAsync(command.OrganizationId, command.RootOrganizationId))
                .ReturnsAsync(organization);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _createEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<OrganizerDoesNotBelongToOrganizationException>();
            _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventCreated>()), Times.Never);
        }
    }
}
