using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Posts.Application.Events.External.Handlers;
using MiniSpace.Services.Posts.Application.Events.External;
using System.ComponentModel.Design;
using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Exceptions;

namespace MiniSpace.Services.Posts.Application.UnitTests.Events.External.Handlers
{
    public class MediaFileDeletedHandlerTest
    {
        private readonly MediaFileDeletedHandler _mediaFileDeletedHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock;

        public MediaFileDeletedHandlerTest()
        {
            _eventRepositoryMock = new();
            _commandDispatcherMock = new();
            _dateTimeProviderMock = new();
            _postRepositoryMock = new Mock<IPostRepository>();
            _mediaFileDeletedHandler = new MediaFileDeletedHandler(
                _postRepositoryMock.Object, _dateTimeProviderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithPostAsSource_ShouldUpdateRepository()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var sourceId = Guid.NewGuid();
            string src = "POST";
            var @event = new MediaFileDeleted(mediaFileId, sourceId, src);
            var cancellationToken = new CancellationToken();
            List<Guid> mediaFiles = [Guid.NewGuid(), mediaFileId, Guid.NewGuid()];

            var post = new Post(sourceId, Guid.NewGuid(), Guid.NewGuid(),
                "text content", mediaFiles, DateTime.Now, State.Published, DateTime.Now);

            _postRepositoryMock.Setup(repo => repo.GetAsync(@event.SourceId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancellationToken);
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithNoMediaFile_ShouldThrowMediaFileNotFoundException()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var sourceId = Guid.NewGuid();
            string src = "POST";
            var @event = new MediaFileDeleted(mediaFileId, sourceId, src);
            var cancellationToken = new CancellationToken();
            List<Guid> mediaFiles = [Guid.NewGuid(), Guid.NewGuid()];
            mediaFiles.RemoveAll(m => m == mediaFileId);

            var post = new Post(sourceId, Guid.NewGuid(), Guid.NewGuid(),
                "text content", mediaFiles, DateTime.Now, State.Published, DateTime.Now);

            _postRepositoryMock.Setup(repo => repo.GetAsync(@event.SourceId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancellationToken);
            await Assert.ThrowsAsync<MediaFileNotFoundException>(act);
        }

        [Fact]
        public async Task HandleAsync_WithSourceNotPost_ShouldNotUpdateRepository()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var sourceId = Guid.NewGuid();
            string src = "garbage";
            var @event = new MediaFileDeleted(mediaFileId, sourceId, src);
            var cancellationToken = new CancellationToken();
            List<Guid> mediaFiles = [Guid.NewGuid(), mediaFileId, Guid.NewGuid()];

            var post = new Post(sourceId, Guid.NewGuid(), Guid.NewGuid(),
                "text content", mediaFiles, DateTime.Now, State.Published, DateTime.Now);

            _postRepositoryMock.Setup(repo => repo.GetAsync(@event.SourceId)).ReturnsAsync(post);

            // Act & Assert
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancellationToken);
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Never());
        }

        [Fact]
        public async Task HandleAsync_WithNullPost_ShouldNotUpdateRepository()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var sourceId = Guid.NewGuid();
            string src = "POST";
            var @event = new MediaFileDeleted(mediaFileId, sourceId, src);
            var cancellationToken = new CancellationToken();
            List<Guid> mediaFiles = [Guid.NewGuid(), mediaFileId, Guid.NewGuid()];

            var post = new Post(sourceId, Guid.NewGuid(), Guid.NewGuid(),
                "text content", mediaFiles, DateTime.Now, State.Published, DateTime.Now);

            _postRepositoryMock.Setup(repo => repo.GetAsync(@event.SourceId)).ReturnsAsync((Post)null);

            // Act & Assert
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancellationToken);
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Never());
        }
    }
}