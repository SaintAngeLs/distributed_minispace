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
    public class AddLikeHandlerTest
    {
        private readonly AddLikeHandler _addLikeHandler;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IEventMapper> _eventMapperMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public AddLikeHandlerTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _eventMapperMock = new Mock<IEventMapper>();
            _appContextMock = new Mock<IAppContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _addLikeHandler = new AddLikeHandler(_commentRepositoryMock.Object, _appContextMock.Object, _messageBrokerMock.Object);
        }

        [Fact]
        public async Task AddLike_WithValidCommentAndAuthorised_ShouldAddLike()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new AddLike(commentId);

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(), CommentContext.Post, Guid.NewGuid(), "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id)).ReturnsAsync(comment);

            var cancelationToken = new CancellationToken();

            // Act
            await _addLikeHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _commentRepositoryMock.Verify(repo => repo.UpdateAsync(comment), Times.Once());
            _eventMapperMock.Verify(mapper => mapper.MapAll(comment.Events), Times.Once());
        }

        [Fact]
        public async Task AddLike_WithInvalidComment_ShouldThrowCommentNotFoundExeption()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new AddLike(commentId);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comand.CommentId)).ReturnsAsync((Comment)null);
            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _addLikeHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<CommentNotFoundException>();
        }

        [Fact]
        public async Task AddLike_WithNonPermitedIdentity_ShouldThrowUnauthorizedCommentAccessException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comand = new AddLike(commentId);

            var comment = Comment.Create(new AggregateId(commentId), Guid.NewGuid(), CommentContext.Post, Guid.NewGuid(), "Adam", Guid.NewGuid(), "text", DateTime.Now);

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", false, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _commentRepositoryMock.Setup(repo => repo.GetAsync(comment.Id)).ReturnsAsync(comment);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _addLikeHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<UnauthorizedCommentAccessException>();

        }
    }
}
