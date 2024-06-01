using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Repositories;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Application.UnitTests.Commands.Handlers
{
    public class AddEventParticipantHandlerTest
    {
        private readonly AddEventParticipantHandler _addEventParticipantHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public AddEventParticipantHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _addEventParticipantHandler = new AddEventParticipantHandler(
                _eventRepositoryMock.Object,
                _studentRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
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
            var command = new AddEventParticipant();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.StudentName = studentName;

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
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            await _addEventParticipantHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventParticipantAdded>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithoutExistingEvent_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new AddEventParticipant();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.StudentName = studentName;

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
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _addEventParticipantHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventParticipantAdded>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WithoWrongStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new AddEventParticipant();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.StudentName = studentName;

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
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _addEventParticipantHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<StudentNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventParticipantAdded>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_NotBeingOrganizer_ShouldThrowUnauthorizedEventAccessException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new AddEventParticipant();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.StudentName = studentName;

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
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _addEventParticipantHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventParticipantAdded>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_NotBeingOrganizerButAdmin_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new AddEventParticipant();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.StudentName = studentName;

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
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _addEventParticipantHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().NotThrowAsync<UnauthorizedEventAccessException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventParticipantAdded>()), Times.Once);
        }
    }
}
