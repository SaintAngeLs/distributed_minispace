using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;

namespace MiniSpace.Services.Posts.Application.UnitTests.Commands.Handlers {
    public class UpdatePostsStateHandlerTest {
        private readonly UpdatePostsStateHandler _updatePostStateHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;

        public UpdatePostsStateHandlerTest() {
            _postRepositoryMock = new();
            _messageBrokerMock = new();
            _updatePostStateHandler = new UpdatePostsStateHandler(_postRepositoryMock.Object,
            _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndStatePublished_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;

            var command = new UpdatePostsState(DateTime.Today);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndStatePublished_ShouldUpdateRepository() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;

            var command = new UpdatePostsState(new DateTime(2024, 1, 1, 0, 0, 0));

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", "Media content", new DateTime(2000, 1, 1, 0, 0, 0),
                state, new DateTime(2000, 1, 1, 0, 0, 0));

            var postList = new List<Post>
            {
                post
            };

            _postRepositoryMock.Setup(repo => repo.GetToUpdateAsync()).ReturnsAsync(postList.AsEnumerable());
            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostStateHandler.HandleAsync(command, cancelationToken);
            await act();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once());
        }
    }
}