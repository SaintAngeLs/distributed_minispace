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
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;

namespace MiniSpace.Services.Posts.Application.UnitTests.Commands.Handlers {
    public class DeletePostHandlerTest {
        private readonly DeletePostHandler _deletePostHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public DeletePostHandlerTest() {
            _postRepositoryMock = new();
            _messageBrokerMock = new();
            _appContextMock = new();
            _deletePostHandler = new DeletePostHandler(_postRepositoryMock.Object,
            _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndStatePublished_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;

            var @event = new Event(eventId, contextId);
            var command = new DeletePost(postId);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today, // TODO: media files
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithNullPost_ShouldThrowPostNotFoundException() {
            // Arrange
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var command = new DeletePost(postId);

            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync((Post)null);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<PostNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithNonPermittedIdentity_ShouldThrowUnauthorizedPostAccessException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;
            var eventId = Guid.NewGuid();

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new DeletePost(postId);

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today, // TODO: media files
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "not admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostAccessException>();
        }

        [Fact]
        public async Task HandleAsync_WithIdentityAsAdminAndForeignPost_ShouldNotThrowException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;
            var eventId = Guid.NewGuid();

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new DeletePost(postId);

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today, // TODO: media files
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "Admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithIdentityAsAdminAndReportedPost_ShouldNotThrowException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Reported;
            var eventId = Guid.NewGuid();

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new DeletePost(postId);

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today, // TODO: media files
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "Admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithIdentityAsNotAdminAndReportedPost_ShouldThrowUnauthorizedPostOperationException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Reported;
            var eventId = Guid.NewGuid();

            var command = new DeletePost(postId);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today, // TODO: media files
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "not admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _deletePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostOperationException>();
        }
    }
}