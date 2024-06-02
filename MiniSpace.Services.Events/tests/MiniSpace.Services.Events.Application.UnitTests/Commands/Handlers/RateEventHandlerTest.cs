using FluentAssertions;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Exceptions;
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
    public class RateEventHandlerTest
    {
        private readonly RateEventHandler _rateEventHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;

        public RateEventHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _rateEventHandler = new RateEventHandler(
                _eventRepositoryMock.Object,
                _studentRepositoryMock.Object );
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new RateEvent();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.Rating = 1;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Published, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            @event.SignUpStudent(new Participant(studentId, "Adam"));
            @event.UpdateState(new DateTime(2026, 1, 1));


            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act
            await _rateEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_WithoutExistingEvent_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new RateEvent();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.Rating = 1;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Published, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            @event.SignUpStudent(new Participant(studentId, "Adam"));
            @event.UpdateState(new DateTime(2026, 1, 1));


            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _rateEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_WithoutExistingStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new RateEvent();
            command.EventId = eventId;
            command.StudentId = studentId;
            command.Rating = 1;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Published, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var student = new Student(studentId);
            @event.SignUpStudent(new Participant(studentId, "Adam"));
            @event.UpdateState(new DateTime(2026, 1, 1));


            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _rateEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<StudentNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}
