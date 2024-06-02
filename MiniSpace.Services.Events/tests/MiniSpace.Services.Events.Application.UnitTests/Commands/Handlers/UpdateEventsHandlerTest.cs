using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Application.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using System.Threading;
using Xunit;
using FluentAssertions;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Application.UnitTests.Commands.Handlers
{
    public class UpdateEventsHandlerTest
    {
        private readonly UpdateEventHandler _updateEventsHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IEventValidator> _eventValidatorMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public UpdateEventsHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventValidatorMock = new Mock<IEventValidator>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _updateEventsHandler = new UpdateEventHandler(
                _eventRepositoryMock.Object,
                _eventValidatorMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object,
                _dateTimeProviderMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new UpdateEvent(eventId, "name", organizerId, 
                new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", string.Empty);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _eventValidatorMock.Setup(val => val.ParseDate(command.PublishDate, "event_publish_date")).Returns(new DateTime(2024, 10, 1));
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            await _updateEventsHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventUpdated>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_WithoutExistingEvent_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new UpdateEvent(eventId, "name", organizerId,
                new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", string.Empty);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            var identityContext = new IdentityContext(organizerId.ToString(),
                "", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);
            _eventValidatorMock.Setup(val => val.ParseDate(command.PublishDate, "event_publish_date")).Returns(new DateTime(2024, 10, 1));
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _updateEventsHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventUpdated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_NotBeingOwner_ShouldThrowUnauthorizedEventAccessException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new UpdateEvent(eventId, "name", organizerId,
                new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", string.Empty);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _eventValidatorMock.Setup(val => val.ParseDate(command.PublishDate, "event_publish_date")).Returns(new DateTime(2024, 10, 1));
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _updateEventsHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventUpdated>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_NotBeingOwnerNutAdmin_ShouldThrowNotUnauthorizedEventAccessException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new UpdateEvent(eventId, "name", organizerId,
                new DateTime(2025, 1, 1).ToString(), new DateTime(2025, 2, 1).ToString(),
                "bname", "street", "1A", "1", "city", "00-000", new List<Guid> { },
                "description", 10, 10, "Music", string.Empty);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "Admin", true, new Dictionary<string, string>());

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _eventValidatorMock.Setup(val => val.ParseDate(command.PublishDate, "event_publish_date")).Returns(new DateTime(2024, 10, 1));
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            _eventValidatorMock.Setup(val => val.ParseDate(command.StartDate, "event_start_date")).Returns(new DateTime(2025, 1, 1));
            _eventValidatorMock.Setup(val => val.ParseDate(command.EndDate, "event_end_date")).Returns(new DateTime(2025, 2, 1));
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);
            _eventValidatorMock.Setup(val => val.ParseCategory(command.Category)).Returns(Category.Music);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _updateEventsHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().NotThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventUpdated>()), Times.Once);
        }
    }
}
