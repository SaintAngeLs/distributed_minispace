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
using MiniSpace.Services.Comments.Application.Events.External.Handlers;
using MiniSpace.Services.Comments.Application.Events.External;
using System.ComponentModel.Design;

namespace MiniSpace.Services.Comments.Application.UnitTests.Events.External.Handlers
{
    public class PostDeletedHandlerTest
    {
        private readonly PostDeletedHandler _postDeletedHandler;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;

        public PostDeletedHandlerTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _postDeletedHandler = new PostDeletedHandler(_commentRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var postId = System.Guid.NewGuid();
            var @event = new PostDeleted(postId);
            var commentId1 = System.Guid.NewGuid();
            var commentId2 = System.Guid.NewGuid();
            var comment1 = Comment.Create(new AggregateId(commentId1), postId,
                CommentContext.Event, System.Guid.NewGuid(), "Adam", System.Guid.NewGuid(), "text", DateTime.Now);
            var comment2 = Comment.Create(new AggregateId(commentId2), postId,
                CommentContext.Event, Guid.NewGuid(), "Bartek", Guid.NewGuid(), "text", DateTime.Now);
            var commentsList = new List<Comment> { comment1, comment2 };

            _commentRepositoryMock.Setup(repo => repo.GetByPostIdAsync(postId))
                .ReturnsAsync(commentsList);

            var cancelationTocken = new CancellationToken();

            // Act
            await _postDeletedHandler.HandleAsync(@event, cancelationTocken);

            // Assert
            _commentRepositoryMock.Verify(repo => repo.DeleteAsync(commentId1), Times.Once());
            _commentRepositoryMock.Verify(repo => repo.DeleteAsync(commentId2), Times.Once());
        }
    }
}
