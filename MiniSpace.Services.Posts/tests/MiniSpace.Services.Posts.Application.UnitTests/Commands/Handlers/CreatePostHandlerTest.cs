using Xunit;
using Moq;
using System;
using System.Collections.Generic;
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
using MiniSpace.Services.Posts.Core.Exceptions;
using Microsoft.OpenApi.Extensions;

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
        public async Task HandleAsync_WithValidParametersAndWithMediaFiles_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published.GetDisplayName();
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndWithoutMediaFiles_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published.GetDisplayName();
            List<Guid> mediafiles = [];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }
        
        [Fact]
        public async Task HandleAsync_WithNonAuthenticated_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published.GetDisplayName();
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state, DateTime.Today);

            var isAuthenticated = false;

            var identityContext = new IdentityContext(contextId.ToString(), "", isAuthenticated, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithNullEvent_ShouldThrowEventNotFoundException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var state = State.Published;
            var cancelationToken = new CancellationToken();
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state.GetDisplayName(), DateTime.Today);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync((Event)null);
            
            // Act
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            
            // Assert
            await act.Should().ThrowAsync<EventNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithTooManyMediaFiles_ShouldThrowInvalidNumberOfPostMediaFilesException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var state = State.Published;
            var cancelationToken = new CancellationToken();
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            
            // Act
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            
            // Assert
            await act.Should().ThrowAsync<InvalidNumberOfPostMediaFilesException>();
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
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, differentOrganizer, "Post", mediafiles,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostCreationAttemptException>();
        }

        [Fact]
        public async Task HandleAsync_WithIdentityNotRelatedToEvent_ShouldThrowUnauthorizedPostCreationAttemptException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var @event = new Event(eventId, differentOrganizer);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state.GetDisplayName(), DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostCreationAttemptException>();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStateName_ShouldThrowInvalidPostStateException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = "a";
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<InvalidPostStateException>();
        }

        [Fact]
        public async Task HandleAsync_WithNewStateReported_ShouldThrowNotAllowedPostStateException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Reported.GetDisplayName();
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<NotAllowedPostStateException>();
        }

        [Fact]
        public async Task HandleAsync_WithStateToBePublishedAndNullPublishDate_ShouldThrowPublishDateNullException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.ToBePublished.GetDisplayName();
            DateTime? publishDate = null;
            List<Guid> mediafiles = [ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() ];

            var @event = new Event(eventId, contextId);
            var command = new CreatePost(postId, eventId, contextId, "Post", mediafiles, state,
                publishDate);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _eventRepositoryMock.Setup(repo => repo.GetAsync(eventId)).ReturnsAsync(@event);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            // Act & Assert
            Func<Task> act = async () => await _createPostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<PublishDateNullException>();
        }
    }
}