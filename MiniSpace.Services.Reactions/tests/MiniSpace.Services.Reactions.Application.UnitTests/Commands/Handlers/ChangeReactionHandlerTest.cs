using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Commands.Handlers;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Infrastructure.Contexts;
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Reactions.Core.Exceptions;
using Microsoft.OpenApi.Extensions;
using System.Security.Policy;

namespace MiniSpace.Services.Reactions.Application.UnitTests.Commands.Handlers {
    public class CreateReactionHandlerTest {
        private readonly CreateReactionHandler _createReactionHandler;
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IEventRepository>  _eventRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public CreateReactionHandlerTest() {
            _postRepositoryMock = new();
            _eventRepositoryMock = new();
            _messageBrokerMock = new();
            _appContextMock = new();
            _reactionRepositoryMock = new();
            _studentRepositoryMock = new();
            _postRepositoryMock = new();
            _createReactionHandler = new CreateReactionHandler(
                _reactionRepositoryMock.Object,
                _postRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _studentRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithInvalidReactionType_ShouldThrowInvalidReactionTypeException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "jkfl;afd", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<InvalidReactionTypeException>(addReactionToEventAction);
        }

        [Fact]
        public async Task HandleAsync_WithAlreadyGivenReaction_ShouldThrowStudentAlreadyGaveReactionException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            _reactionRepositoryMock.Setup(repo =>
                repo.ExistsAsync(eventId, ReactionContentType.Event, studentId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<StudentAlreadyGaveReactionException>(addReactionToEventAction);
        }

        [Fact]
        public async Task HandleAsync_WithValidParameters_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await addReactionToEventAction.Should().NotThrowAsync();
            await addReactionToPostAction.Should().NotThrowAsync();

            _messageBrokerMock.Verify(broker => broker.PublishAsync(It.IsAny<ReactionCreated>()), Times.Exactly(2));
            _reactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Reaction>()), Times.Exactly(2));
        }

        [Fact]
        public async Task HandleAsync_WithIdentityNotRelatedToEvent_ShouldThrowUnauthorizedReactionAccessException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            Guid contextId;
            do {
                contextId = Guid.NewGuid();
            } while (contextId == studentId);

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");
            
            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);

            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);

            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);

            await Assert.ThrowsAsync<UnauthorizedReactionAccessException>(addReactionToEventAction);
        }

        [Fact]
        public async Task HandleAsync_WithIdentityNotRelatedToPost_ShouldThrowUnauthorizedReactionAccessException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            Guid contextId;
            do {
                contextId = Guid.NewGuid();
            } while (contextId == studentId);

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);

            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);

            await Assert.ThrowsAsync<UnauthorizedReactionAccessException>(addReactionToPostAction);
        }

        [Fact]
        public async Task HandleAsync_WithNonAuthenticated_ShouldNotThrowException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await addReactionToEventAction.Should().NotThrowAsync();
            await addReactionToPostAction.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithStudentNotInRepository_ShouldThrowStudentNotFoundException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(false);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<StudentNotFoundException>(addReactionToEventAction);
            await Assert.ThrowsAsync<StudentNotFoundException>(addReactionToPostAction);
        }

        [Fact]
        public async Task HandleAsync_WithInvalidEventReactionContentType_ShouldThrowInvalidReactionContentTypeException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "fdjkacrogc");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "fchoraegc");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<InvalidReactionContentTypeException>(addReactionToEventAction);
        }

        [Fact]
        public async Task HandleAsync_WithInvalidPostReactionContentType_ShouldThrowInvalidReactionContentTypeException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "fdjkacrogc");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "fchoraegc");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<InvalidReactionContentTypeException>(addReactionToPostAction);
        }

        [Fact]
        public async Task HandleAsync_WithEventNotInRepository_ShouldThrowEventNotFoundException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(true);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(false);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<EventNotFoundException>(addReactionToEventAction);
        }

        [Fact]
        public async Task HandleAsync_WithPostNotInRepository_ShouldThrowPostNotFoundException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;

            var command_evt = new CreateReaction(reactionId, studentId, eventId, "LikeIt", "Event");
            var command_post = new CreateReaction(reactionId, studentId, postId, "LikeIt", "Post");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId)).ReturnsAsync(false);
            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId)).ReturnsAsync(true);
            
            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> addReactionToEventAction = async () =>
                await _createReactionHandler.HandleAsync(command_evt, cancelationToken);
            
            Func<Task> addReactionToPostAction = async () =>
                await _createReactionHandler.HandleAsync(command_post, cancelationToken);
            
            await Assert.ThrowsAsync<PostNotFoundException>(addReactionToPostAction);
        }


    }
}