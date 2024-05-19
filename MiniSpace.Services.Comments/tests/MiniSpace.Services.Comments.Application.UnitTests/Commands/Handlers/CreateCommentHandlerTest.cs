using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;

namespace MiniSpace.Services.Comments.Application.UnitTests.Commands.Handlers
{
    public class CreateCommentHandlerTest
    {
        private readonly CreateCommentHandler _createCommentHandler;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IEventMapper> _eventMapperMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public CreateCommentHandlerTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _eventMapperMock = new Mock<IEventMapper>();
            _appContextMock = new Mock<IAppContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _createCommentHandler = new CreateCommentHandler(
                _commentRepositoryMock.Object,
                _studentRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _messageBrokerMock.Object,
                _appContextMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithVaildStudentAndContextNoParent_ShouldNotThrowExeption()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.Empty;
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                true, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithVaildStudentAndContextandOneParent_ShouldNotThrowExeptionAndUpdateParent()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var parentComment = Comment.Create(new AggregateId(parentId), contextId,
                CommentContext.Post, Guid.NewGuid(), "alex", Guid.Empty, "text", DateTime.Now);
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                true, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(parentId))
                .ReturnsAsync(parentComment);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().NotThrowAsync();
            _commentRepositoryMock.Verify(repo => repo.UpdateAsync(parentComment), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithNonPermitedIdentity_ShouldThrowUnauthorizedCommentAccessException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.Empty;
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "",
                true, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().ThrowAsync<UnauthorizedCommentAccessException>();
        }

        [Fact]
        public async Task HandleAsync_WithWrongStudentId_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.Empty;
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                false, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithWrongCommentContextEnum_ShouldThrowInvalidCommentContextEnumException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.Empty;
            var command = new CreateComment(commentId, contextId, "wrongenumstring", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                false, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().ThrowAsync<InvalidCommentContextEnumException>();
        }

        [Fact]
        public async Task HandleAsync_WithWrongParentId_ShouldThrowParentCommentNotFoundException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var parentComment = Comment.Create(new AggregateId(parentId), contextId,
                CommentContext.Post, Guid.NewGuid(), "alex", Guid.Empty, "text", DateTime.Now);
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                false, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(parentId))
                .ReturnsAsync((Comment)null);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().ThrowAsync<ParentCommentNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithParentThatHasAParent_ShouldThrowInvalidParentCommentException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var contextId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var parentComment = Comment.Create(new AggregateId(parentId), contextId,
                CommentContext.Post, Guid.NewGuid(), "alex", Guid.NewGuid(), "text", DateTime.Now);
            var command = new CreateComment(commentId, contextId, "Post", studentId,
                parentId, "text");
            var cancelationTocken = new CancellationToken();

            var identityContext = new IdentityContext(studentId.ToString(), "",
                true, new Dictionary<string, string>());
            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(parentId))
                .ReturnsAsync(parentComment);

            // Act & Assert
            Func<Task> act = async () => await _createCommentHandler.HandleAsync(command, cancelationTocken);
            await act.Should().ThrowAsync<InvalidParentCommentException>();
        }
    }
}
