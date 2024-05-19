using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;

namespace MiniSpace.Services.Comments.Application.UnitTests.Commands.Handlers
{
    public class UpdateCommnetHandlerTest
    {
        public readonly UpdateCommentHandler _updateCommentHandler;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public UpdateCommnetHandlerTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _appContextMock = new Mock<IAppContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _updateCommentHandler = new UpdateCommentHandler(
                _commentRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object,
                _dateTimeProviderMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithVaildStudentAndPermited_ShouldUpdateRepository()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new UpdateComment(commentId, "newText");
            var studentId = Guid.NewGuid();

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(),
                CommentContext.Post, studentId, "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(studentId.ToString(),
                "", true, new Dictionary<string, string>());
            
            var dateTimeNow = DateTime.Now;

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id))
                .ReturnsAsync(comment);
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(dateTimeNow);

            var cancelationToken = new CancellationToken();

            // Act
            await _updateCommentHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _commentRepositoryMock.Verify(repo => repo.UpdateAsync(comment), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithInvalidComment_ShouldThrowCommentNotFoundExeption()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new UpdateComment(commentId, "newText");
            var studentId = Guid.NewGuid();

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(),
                CommentContext.Post, studentId, "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(studentId.ToString(),
                "", true, new Dictionary<string, string>());

            var dateTimeNow = DateTime.Now;

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id))
                .ReturnsAsync((Comment)null);
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(dateTimeNow);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _updateCommentHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<CommentNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithAdminNotTheirs_ShouldNotThrowException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new UpdateComment(commentId, "newText");
            var studentId = Guid.NewGuid();

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(),
                CommentContext.Post, studentId, "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "Admin", true, new Dictionary<string, string>());

            var dateTimeNow = DateTime.Now;

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id))
                .ReturnsAsync(comment);
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(dateTimeNow);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _updateCommentHandler.HandleAsync(comand, cancelationToken);
            await act.Should().NotThrowAsync();

        }

        [Fact]
        public async Task HandleAsync_WithNotAdminNotTheirs_ShouldThrowUnauthorizedCommentAccessException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new UpdateComment(commentId, "newText");
            var studentId = Guid.NewGuid();

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(),
                CommentContext.Post, studentId, "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "", true, new Dictionary<string, string>());

            var dateTimeNow = DateTime.Now;

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id))
                .ReturnsAsync(comment);
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(dateTimeNow);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _updateCommentHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedCommentAccessException>();

        }
    }
}
