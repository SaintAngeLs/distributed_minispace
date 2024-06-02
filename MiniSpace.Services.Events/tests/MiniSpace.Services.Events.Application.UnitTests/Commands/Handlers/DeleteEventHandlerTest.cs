using FluentAssertions;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
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
    public class DeleteEventHandlerTest
    {
        private readonly DeleteEventHandler _deleteEventHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public DeleteEventHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _deleteEventHandler = new DeleteEventHandler(
                _eventRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object );
        }
        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new DeleteEvent();
            command.EventId = eventId;

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
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            await _deleteEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(eventId), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventDeleted>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_WherEventDoesNotExists_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new DeleteEvent();
            command.EventId = eventId;

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
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(eventId), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventDeleted>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_WithoutBeingOwner_ShouldThrowUnauthorizedEventAccessException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new DeleteEvent();
            command.EventId = eventId;

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
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(eventId), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventDeleted>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_WithoutBeingOwnerbutAdmin_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new DeleteEvent();
            command.EventId = eventId;

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
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().NotThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.DeleteAsync(eventId), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventDeleted>()), Times.Once);
        }
    }
}
