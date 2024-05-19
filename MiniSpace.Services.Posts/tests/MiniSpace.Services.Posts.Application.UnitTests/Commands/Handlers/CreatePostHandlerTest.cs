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
    public class CreatePostHandlerTest {
        private readonly CreatePostHandler _createPostHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IEventRepository>  _eventRepositoryMock;
        private readonly Mock<IDateTimeProvider>  _dateTimeProviderMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public CreatePostHandlerTest() {
            _postRepositoryMock = new();
            _eventRepositoryMock = new();
            _dateTimeProviderMock = new();
            _messageBrokerMock = new();
            _appContextMock = new();
            _createPostHandler = new CreatePostHandler(_postRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _messageBrokerMock.Object,
                _appContextMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithNonPermittedIdentity_ShouldThrowUnauthorizedPostCreationAttemptException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", "Media Content",
                nameof(state), DateTime.Today);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity.IsAuthenticated).Returns(true);
            _appContextMock.Setup(ctx => ctx.Identity.Id).Should().NotBeEquivalentTo(command.OrganizerId);
            _appContextMock.Setup(ctx => ctx.Identity.IsAdmin).Returns(false);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostCreationAttemptException>();
        }
    }
}