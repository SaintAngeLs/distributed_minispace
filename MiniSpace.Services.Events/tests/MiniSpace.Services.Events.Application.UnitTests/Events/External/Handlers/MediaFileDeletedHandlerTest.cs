using Microsoft.Extensions.Logging;
using MiniSpace.Services.Events.Application.Events.External;
using MiniSpace.Services.Events.Application.Events.External.Handlers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Events.Application.UnitTests.Events.External.Handlers
{
    public class MediaFileDeletedHandlerTest
    {
        private readonly MediaFileDeletedHandler _mediaFileDeletedHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;

        public MediaFileDeletedHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mediaFileDeletedHandler = new MediaFileDeletedHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var source = "event";
            var command = new MediaFileDeleted(mediaFileId, eventId, source);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { mediaFileId }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            var student = new Student(studentId);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);

            var cancelationToken = new CancellationToken();

            // Act
            await _mediaFileDeletedHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_WithoutExistingEvent_ShouldNotUpdateAnything()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var source = "event";
            var command = new MediaFileDeleted(mediaFileId, eventId, source);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            var student = new Student(studentId);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);

            var cancelationToken = new CancellationToken();

            // Act
            await _mediaFileDeletedHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
        [Fact]
        public async Task HandleAsync_NotEvent_ShouldIgnore()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var source = "notevent";
            var command = new MediaFileDeleted(mediaFileId, eventId, source);

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            var student = new Student(studentId);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);

            var cancelationToken = new CancellationToken();

            // Act
            await _mediaFileDeletedHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}
