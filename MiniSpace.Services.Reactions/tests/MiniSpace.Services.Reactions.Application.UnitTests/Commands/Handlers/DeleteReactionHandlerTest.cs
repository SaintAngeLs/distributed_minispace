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
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Reactions.Core.Exceptions;
using Microsoft.OpenApi.Extensions;
using System.Security.Policy;

namespace MiniSpace.Services.Reactions.Application.UnitTests.Commands.Handlers {
    public class DeleteReactionHandlerTest {
        private readonly DeleteReactionHandler _deleteReactionHandler;
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public DeleteReactionHandlerTest() {
            _messageBrokerMock = new();
            _appContextMock = new();
            _reactionRepositoryMock = new();
            _deleteReactionHandler = new DeleteReactionHandler(
                _reactionRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object);
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
            var command = new DeleteReaction(reactionId);

            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            var reaction = new Reaction(reactionId, studentId, "full name", ReactionType.HateIt,
                Guid.NewGuid(), ReactionContentType.Event);

            _reactionRepositoryMock.Setup(repo => repo.GetAsync(reactionId)).ReturnsAsync(reaction);
            _appContextMock.Setup(cxt => cxt.Identity).Returns(identity);

            // Act & Assert
            Func<Task> act = async () =>
                await _deleteReactionHandler.HandleAsync(command, cancelationToken);
            
            await act.Should().NotThrowAsync();

            _messageBrokerMock.Verify(broker => broker.PublishAsync(It.IsAny<ReactionDeleted>()), Times.Exactly(1));
            _reactionRepositoryMock.Verify(repo => repo.DeleteAsync(reactionId), Times.Exactly(1));
        }

        [Fact]
        public async Task HandleAsync_WithReactionNotInRepository_ShouldThrowReactionNotFoundException() {
            // Arrange
            var eventId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var reactionId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var cancelationToken = new CancellationToken();

            var contextId = studentId;
            var command = new DeleteReaction(reactionId);

            var reaction = new Reaction(reactionId, studentId, "full name", ReactionType.HateIt,
                Guid.NewGuid(), ReactionContentType.Event);

            _reactionRepositoryMock.Setup(repo => repo.GetAsync(reactionId)).ReturnsAsync((Reaction)null);

            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> act = async () =>
                await _deleteReactionHandler.HandleAsync(command, cancelationToken);
            
            await Assert.ThrowsAsync<ReactionNotFoundException>(act);
        }

        [Fact]
        public async Task HandleAsync_WithForeignOwner_ShouldThrowUnauthorizedReactionAccessException() {
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
            var command = new DeleteReaction(reactionId);

            var reaction = new Reaction(reactionId, studentId, "full name", ReactionType.HateIt,
                Guid.NewGuid(), ReactionContentType.Event);

            _reactionRepositoryMock.Setup(repo => repo.GetAsync(reactionId)).ReturnsAsync(reaction);

            var identity = new IdentityContext(contextId.ToString(), "user", true, default);
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identity);

            // Act & Assert
            Func<Task> act = async () =>
                await _deleteReactionHandler.HandleAsync(command, cancelationToken);
            
            await Assert.ThrowsAsync<UnauthorizedReactionAccessException>(act);
        }
    }
}