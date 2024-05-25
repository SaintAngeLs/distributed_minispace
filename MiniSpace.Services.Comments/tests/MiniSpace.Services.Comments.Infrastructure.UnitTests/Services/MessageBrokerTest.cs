using Xunit;
using Moq;
using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;
using MiniSpace.Services.Comments.Infrastructure.Services;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Application.Events;

namespace MiniSpace.Services.Comments.Infrastructure.UnitTests.Services
{
    public class MessageBrokerTest
    {
        private readonly MessageBroker _messageBroker;
        private readonly Mock<IBusPublisher> _mockBusPublisher;
        private readonly Mock<IMessageOutbox> _mockMessageOutbox;
        private readonly Mock<ICorrelationContextAccessor> _mockContextAccessor;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IMessagePropertiesAccessor> _mockMessagePropertiesAccessor;
        private readonly Mock<ITracer> _mockTracer;
        private readonly Mock<ILogger<IMessageBroker>> _mockLogger;

        public MessageBrokerTest()
        {
            _mockBusPublisher = new Mock<IBusPublisher>();
            _mockMessageOutbox = new Mock<IMessageOutbox>();
            _mockContextAccessor = new Mock<ICorrelationContextAccessor>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockMessagePropertiesAccessor = new Mock<IMessagePropertiesAccessor>();
            _mockTracer = new Mock<ITracer>();
            _mockLogger = new Mock<ILogger<IMessageBroker>>();

            _messageBroker = new MessageBroker(_mockBusPublisher.Object, _mockMessageOutbox.Object, _mockContextAccessor.Object,
                _mockHttpContextAccessor.Object, _mockMessagePropertiesAccessor.Object, new RabbitMqOptions(),
                _mockTracer.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task PublishAsync_WithEventsAndOutboxDisabled_PublishesEvents()
        {
            //Arrange
            var events = new List<IEvent>
            {
                new CommentCreated(Guid.NewGuid())
            };
            _mockMessageOutbox.Setup(x => x.Enabled).Returns(false);

            //Act
            await _messageBroker.PublishAsync(events);

            //Assert
            _mockMessageOutbox.Verify(
                x => x.SendAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()),
                Times.Never
            );
            _mockBusPublisher.Verify(
                x => x.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(events.Count)
            );
        }

        [Fact]
        public async Task PublishAsync_WithEventsAndOutboxEnabled_SendsMessagesToOutbox()
        {
            //Arrange
            var events = new List<IEvent>
            {
                new CommentCreated(Guid.NewGuid())
            };
            _mockMessageOutbox.Setup(x => x.Enabled).Returns(true);

            //Act
            await _messageBroker.PublishAsync(events);

            //Assert
            _mockMessageOutbox.Verify(
                x => x.SendAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()),
                Times.Exactly(events.Count)
            );
            _mockBusPublisher.Verify(
                x => x.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Never
            );
        }

        [Fact]
        public async Task PublishAsync_WithoutEvents_Returns()
        {
            //Arrange
            List<IEvent> events = null;

            //Act
            await _messageBroker.PublishAsync(events);

            //Assert
            _mockMessageOutbox.Verify(
                x => x.SendAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()),
                Times.Never
                );
            _mockBusPublisher.Verify(
                x => x.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Never
                    );
        }

        [Fact]
        public async Task PublishAsync_WithNullEventAndOutboxDisabled_PublishesOneLessEvent()
        {
            //Arrange
            var events = new List<IEvent>
            {
                new CommentCreated(Guid.NewGuid()),
                null
            };
            _mockMessageOutbox.Setup(x => x.Enabled).Returns(false);

            //Act
            await _messageBroker.PublishAsync(events);

            //Assert
            _mockMessageOutbox.Verify(
                x => x.SendAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()),
                Times.Never
                );
            _mockBusPublisher.Verify(
                x => x.PublishAsync(It.IsAny<IEvent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(events.Count - 1)
                    );
        }
    }
}
