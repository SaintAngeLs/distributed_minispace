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
using MiniSpace.Services.Posts.Infrastructure.Services;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Infrastructure.Services.Workers;
using Convey.CQRS.Commands;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using App.Metrics.Timer;
using MiniSpace.Services.Posts.Application.Commands;

namespace MiniSpace.Services.Posts.Infrastructure.UnitTests.Services.Workers
{
    public class PostStateUpdaterWorkerTest
    {
        private readonly PostStateUpdaterWorker _postStateUpdaterWorker;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public PostStateUpdaterWorkerTest()
        {
            _messageBrokerMock = new Mock<IMessageBroker>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _postStateUpdaterWorker = new PostStateUpdaterWorker(_messageBrokerMock.Object,
                _commandDispatcherMock.Object, _dateTimeProviderMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithDefaultParameters_ShouldPublishStarted()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await _postStateUpdaterWorker.StartAsync(cancellationToken);
            await Task.Delay(1000);

            // Assert
            _messageBrokerMock.Verify(broker =>
                broker.PublishAsync(It.IsAny<PostBackgroundWorkerStarted>()),
                Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_WithCancelRequested_ShouldPublishStopped()
        {
            // Arrange
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await _postStateUpdaterWorker.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await _postStateUpdaterWorker.StopAsync(cancellationToken);

            // Assert
            _messageBrokerMock.Verify(broker =>
                broker.PublishAsync(It.IsAny<PostBackgroundWorkerStopped>()),
                Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_WithTimeSetToInvokeCommandDispatcherSendAsync_ShouldCommandDispatcherSendAsync()
        {
            CancellationToken cancellationToken = new CancellationToken();
            var nowMock0 = new DateTime(2024, 5, 26, 18, PostStateUpdaterWorker.MinutesInterval * 1, 0);
            var nowMock1 = new DateTime(2024, 5, 26, 18, PostStateUpdaterWorker.MinutesInterval * 2, 0);
            
            _dateTimeProviderMock.Setup(prov => prov.Now).Returns(nowMock0);
            await _postStateUpdaterWorker.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await _postStateUpdaterWorker.StopAsync(cancellationToken);

            _dateTimeProviderMock.Setup(prov => prov.Now).Returns(nowMock1);
            await _postStateUpdaterWorker.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await _postStateUpdaterWorker.StopAsync(cancellationToken);

            _commandDispatcherMock.Verify(broker =>
                broker.SendAsync(It.IsAny<UpdatePostsState>(), It.IsAny<CancellationToken>()),
                Times.Exactly(2));
        }
    }
}