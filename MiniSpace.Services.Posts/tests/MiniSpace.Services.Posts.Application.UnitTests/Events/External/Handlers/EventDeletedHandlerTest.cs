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
using Convey.CQRS.Commands;

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

        // TODO: connect ICommandDispatcher with IPostRepository

        // [Fact]
        // public async Task HandleAsync_ValidData_ShouldNotThrowException()
        // {
        //     // Arrange
        //     var socialEventId = Guid.NewGuid();
        //     var organizerId = Guid.NewGuid();
        //     var socialEvent = new Event(socialEventId, organizerId);
        //     var serviceEvent = new EventDeleted(socialEventId);
        //     var postId1 = Guid.NewGuid();
        //     var postId2 = Guid.NewGuid();
        //     var post1 = Post.Create(new AggregateId(postId1), socialEventId, Guid.NewGuid(),
        //         "a", "a", DateTime.Today,
        //         State.Published, DateTime.Today);
        //     var post2 = Post.Create(new AggregateId(postId2), socialEventId, Guid.NewGuid(),
        //         "a", "a", DateTime.Today,
        //         State.Published, DateTime.Today);
        //     var postsList = new List<Post> { post1, post2 };

        //     _postRepositoryMock.Setup(repo => repo.GetByEventIdAsync(socialEventId))
        //         .ReturnsAsync(postsList.AsEnumerable());

        //     var cancellationToken = new CancellationToken();

        //     _eventRepositoryMock.Setup(repo => repo.ExistsAsync(socialEventId)).ReturnsAsync(true);
        //     _eventRepositoryMock.Setup(repo => repo.GetAsync(socialEventId)).ReturnsAsync(socialEvent);

        //     // Act
        //     await _eventDeletedHandler.HandleAsync(serviceEvent, cancellationToken);

        //     // Assert
        //     _postRepositoryMock.Verify(repo => repo.DeleteAsync(postId1), Times.Once());
        //     _postRepositoryMock.Verify(repo => repo.DeleteAsync(postId2), Times.Once());
        // }

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