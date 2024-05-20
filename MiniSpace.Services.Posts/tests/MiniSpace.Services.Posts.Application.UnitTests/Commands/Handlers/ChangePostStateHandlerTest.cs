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
using Microsoft.OpenApi.Extensions;
using MiniSpace.Services.Posts.Core.Exceptions;
using System.Diagnostics.Eventing.Reader;

namespace MiniSpace.Services.Posts.Application.UnitTests.Commands.Handlers {
    public class ChangePostStateHandlerTest {
        private readonly ChangePostStateHandler _changePostStateHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IDateTimeProvider>  _dateTimeProviderMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public ChangePostStateHandlerTest() {
            _postRepositoryMock = new();
            _messageBrokerMock = new();
            _dateTimeProviderMock = new();
            _appContextMock = new();
            _changePostStateHandler = new ChangePostStateHandler(_postRepositoryMock.Object,
            _appContextMock.Object,
            _dateTimeProviderMock.Object,
            _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndStateToBePublished_ShouldNotThrowException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;
            var eventId = Guid.NewGuid();

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var post = Post.Create(new AggregateId(postId), eventId, contextId, "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithNullPost_ShouldThrowPostNotFoundException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;
            var eventId = Guid.NewGuid();

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync((Post)null);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<PostNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithNonAuthenticated_ShouldNotThrowException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished;
            var eventId = Guid.NewGuid();

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", false, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var post = Post.Create(new AggregateId(postId), eventId, contextId, "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
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

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "not admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
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

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "Admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidState_ShouldThrowInvalidPostStateException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = "a";
            var eventId = Guid.NewGuid();

            var command = new ChangePostState(postId,
                state, DateTime.Today);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", "Media content", DateTime.Today,
                State.ToBePublished, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<InvalidPostStateException>();
        }

        [Fact]
        public async Task HandleAsync_WithSameState_ShouldThrowPostStateAlreadySetException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var eventId = Guid.NewGuid();
            var state = State.Published;

            var command = new ChangePostState(postId,
                state.GetDisplayName(), DateTime.Today);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<PostStateAlreadySetException>();
        }

        [Fact]
        public async Task HandleAsync_WithStateToBePublishedAndNullPublishDate_ShouldThrowPublishDateNullException() {
            // Arrange
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var eventId = Guid.NewGuid();
            var state = State.ToBePublished;
            DateTime? publishDate = null;

            var command = new ChangePostState(postId,
                state.GetDisplayName(), publishDate);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", "Media content", DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _changePostStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<PublishDateNullException>();
        }
    }
}