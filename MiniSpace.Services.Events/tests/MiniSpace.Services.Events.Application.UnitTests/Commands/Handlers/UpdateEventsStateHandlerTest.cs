using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Events;
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
    public class UpdateEventsStateHandlerTest
    {
        private readonly UpdateEventsStateHandler _updateEventsStateHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public UpdateEventsStateHandlerTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _updateEventsStateHandler = new UpdateEventsStateHandler(
                _eventRepositoryMock.Object,
                _messageBrokerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var command = new UpdateEventsState(new DateTime(2025, 1, 1));

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);


            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Event> { @event });

            var cancelationToken = new CancellationToken();

            // Act
            await _updateEventsStateHandler.HandleAsync(command, cancelationToken);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Once);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<EventsStateUpdated>()), Times.Once);
        }
    }
}
