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

namespace MiniSpace.Services.Posts.Application.UnitTests.Events.External.Handlers
{
    public class EventCreatedHandlerTest
    {
        private readonly EventCreatedHandler _eventDeletedHandler;
        private readonly Mock<IEventRepository> _eventRepositoryMock;

        public EventCreatedHandlerTest()
        {
            _eventRepositoryMock = new();
            _eventDeletedHandler = new EventCreatedHandler(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var @event = new EventCreated(eventId, organizerId);

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _eventDeletedHandler.HandleAsync(@event);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_EventAlreadyCreated_ShouldThrowEventAlreadyExistsException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var @event = new EventCreated(eventId, organizerId);

            _eventRepositoryMock.Setup(repo => repo.ExistsAsync(eventId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _eventDeletedHandler.HandleAsync(@event);
            await act.Should().ThrowAsync<EventAlreadyAddedException>();
        }
    }
}