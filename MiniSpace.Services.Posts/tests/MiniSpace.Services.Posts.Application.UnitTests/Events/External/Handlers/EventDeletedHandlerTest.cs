using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Posts.Application.Events.External.Handlers;
using MiniSpace.Services.Posts.Application.Events.External;
using System.ComponentModel.Design;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.UnitTests.Events.External.Handlers
{
    public class EventDeletedHandlerTest
    {
        private readonly EventDeletedHandler _eventDeletedHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock;

        public EventDeletedHandlerTest()
        {
            _eventRepositoryMock = new();
            _commandDispatcherMock = new();
            _postRepositoryMock = new Mock<IPostRepository>();
            _eventDeletedHandler = new EventDeletedHandler(_eventRepositoryMock.Object,
                _postRepositoryMock.Object, _commandDispatcherMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var @event = new EventDeleted(eventId);

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _eventDeletedHandler.HandleAsync(@event);
            await act.Should().NotThrowAsync();
            
        }

        [Fact]
        public async Task HandleAsync_NullEvent_ShouldThrowEventNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var @event = new EventDeleted(eventId);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);
            
            // Act
            Func<Task> act = async () => await _eventDeletedHandler.HandleAsync(@event, cancelationToken);
            
            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
        }
    }
}