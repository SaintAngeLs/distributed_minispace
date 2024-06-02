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
    public class UpdatePostHandlerTest {
        private readonly UpdatePostHandler _updatePostHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IDateTimeProvider>  _dateTimeProviderMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public UpdatePostHandlerTest() {
            _postRepositoryMock = new();
            _dateTimeProviderMock = new();
            _messageBrokerMock = new();
            _appContextMock = new();
            _updatePostHandler = new UpdatePostHandler(_postRepositoryMock.Object,
            _appContextMock.Object,
            _messageBrokerMock.Object,
                _dateTimeProviderMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndWithMediaFiles_ShouldUpdateRepository() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            var @event = new Event(eventId, contextId);
            var command = new UpdatePost(postId, textContent, newMediaFiles);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", oldMediaFiles, DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithTooManyMediaFiles_ShouldThrowInvalidNumberOfPostMediaFilesException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            var @event = new Event(eventId, contextId);
            var command = new UpdatePost(postId, textContent, newMediaFiles);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", oldMediaFiles, DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
            await Assert.ThrowsAsync<InvalidNumberOfPostMediaFilesException>(act);
        }

        [Fact]
        public async Task HandleAsync_WithValidParametersAndWithoutMediaFiles_ShouldUpdateRepository() {
            // Arrange
            var eventId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var state = State.Published;
            var textContent = "a";

            var @event = new Event(eventId, contextId);
            var command = new UpdatePost(postId, textContent, Enumerable.Empty<Guid>());

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", Enumerable.Empty<Guid>(), DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithNullPost_ShouldThrowPostNotFoundException() {
            // Arrange
            var postId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            var command = new UpdatePost(postId, textContent, newMediaFiles);

            _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync((Post)null);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
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
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new UpdatePost(postId, textContent, newMediaFiles); 

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", oldMediaFiles, DateTime.Today, 
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "not admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
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
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new UpdatePost(postId, textContent, newMediaFiles); 

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", oldMediaFiles, DateTime.Today, 
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "Admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
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
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            Guid differentOrganizer;
            do {
                differentOrganizer = Guid.NewGuid();
            } while (differentOrganizer == contextId);

            var command = new UpdatePost(postId, textContent, newMediaFiles); 

            var post = Post.Create(new AggregateId(postId), eventId, differentOrganizer,
                "Text", oldMediaFiles, DateTime.Today, 
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "Admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
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
            var textContent = "a";
            List<Guid> oldMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];
            List<Guid> newMediaFiles = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

            var command = new UpdatePost(postId, textContent, newMediaFiles);

            var post = Post.Create(new AggregateId(postId), eventId, contextId,
                "Text", oldMediaFiles, DateTime.Today,
                state, DateTime.Today);

            var identityContext = new IdentityContext(contextId.ToString(), "not admin", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _postRepositoryMock.Setup(repo => repo.GetAsync(command.PostId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _updatePostHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedPostOperationException>();
        }
    }
}