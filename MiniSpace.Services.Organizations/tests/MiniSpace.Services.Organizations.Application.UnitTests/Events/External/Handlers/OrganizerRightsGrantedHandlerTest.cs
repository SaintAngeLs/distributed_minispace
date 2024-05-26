using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Organizations.Application.Commands.Handlers;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Infrastructure.Contexts;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Events.External.Handlers;
using MiniSpace.Services.Organizations.Application.Events.External;

namespace MiniSpace.Services.Organizations.Application.UnitTests.Events.External.Handlers
{
    public class OrganizerRightsGrantedHandlerTest
    {
        private readonly OrganizerRightsGrantedHandler _organizerRightsGrantedHandler;
        private readonly Mock<IOrganizerRepository> _organizerRepository;

        public OrganizerRightsGrantedHandlerTest()
        {
            _organizerRepository = new Mock<IOrganizerRepository>();
            _organizerRightsGrantedHandler = new OrganizerRightsGrantedHandler(_organizerRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidOrganizer_ShouldNotThrow()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var @event = new OrganizerRightsGranted(userId);

            _organizerRepository.Setup(repo => repo.ExistsAsync(userId)).ReturnsAsync(false);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _organizerRightsGrantedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOrganizer_ShouldThrowOrganizerAlreadyAddedException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var @event = new OrganizerRightsGranted(userId);

            _organizerRepository.Setup(repo => repo.ExistsAsync(userId)).ReturnsAsync(true);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _organizerRightsGrantedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().ThrowAsync<OrganizerAlreadyAddedException>();
        }
    }
}
