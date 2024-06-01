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
    public class CancelRateEventHandlerTest
    {
        private readonly CancelRateEventHandler _cancelRateEventHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;

        public CancelRateEventHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _cancelRateEventHandler = new CancelRateEventHandler(
                _eventRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new CancelRateEvent();
            command.EventId = eventId;
            command.StudentId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Published, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            @event.SignUpStudent(new Participant(studentId, "Adam"));
            @event.UpdateState(new DateTime(2026, 1, 1));
            @event.Rate(studentId, 1);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);

            var cancelationToken = new CancellationToken();

            // Act
            await _cancelRateEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_WithoutValidEvent_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new CancelRateEvent();
            command.EventId = eventId;
            command.StudentId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Published, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            @event.SignUpStudent(new Participant(studentId, "Adam"));
            @event.UpdateState(new DateTime(2026, 1, 1));
            @event.Rate(studentId, 1);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _cancelRateEventHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}
