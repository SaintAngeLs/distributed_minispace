using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Commands.Handlers;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Reactions.Application.Events.External.Handlers;
using MiniSpace.Services.Reactions.Application.Events.External;
using System.ComponentModel.Design;
using Convey.CQRS.Commands;
using MiniSpace.Services.Reactions.Application.Events;

namespace MiniSpace.Services.Reactions.Application.UnitTests.Events.External.Handlers
{
    public class PostCreatedHandlerTest
    {
        private readonly PostCreatedHandler _postDeletedHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;

        public PostCreatedHandlerTest()
        {
            _postRepositoryMock = new();
            _postDeletedHandler = new PostCreatedHandler(_postRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new PostCreated(postId);

            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _postDeletedHandler.HandleAsync(post, new CancellationToken());
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_PostAlreadyCreated_ShouldThrowPostAlreadyExistsException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new PostCreated(postId);

            _postRepositoryMock.Setup(repo => repo.ExistsAsync(postId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _postDeletedHandler.HandleAsync(post, new CancellationToken());
            await act.Should().ThrowAsync<PostAlreadyAddedException>();
        }
    }
}