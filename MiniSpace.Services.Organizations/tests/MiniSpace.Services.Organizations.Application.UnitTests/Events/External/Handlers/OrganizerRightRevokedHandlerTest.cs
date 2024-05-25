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
    public class OrganizerRightRevokedHandlerTest
    {
        private readonly OrganizerRightsRevokedHandler _organizerRightsRevokedHandler;
        private readonly Mock<IOrganizerRepository> _organizerRepository;
        private readonly Mock<IOrganizationRepository> _organizationRepository;

        public OrganizerRightRevokedHandlerTest()
        {
            _organizerRepository = new Mock<IOrganizerRepository>();
            _organizationRepository = new Mock<IOrganizationRepository>();
            _organizerRightsRevokedHandler = new OrganizerRightsRevokedHandler(_organizerRepository.Object, _organizationRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidOrganizer_ShouldNotThrow()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var @event = new OrganizerRightsRevoked(userId);
            var organizer = new Organizer(userId);
            var organization = new Organization(Guid.NewGuid(), "name", new List<Organizer>() { organizer }, new List<Organization>() { });

            _organizerRepository.Setup(repo => repo.GetAsync(@event.UserId)).ReturnsAsync(organizer);
            _organizationRepository.Setup(repo => repo.GetOrganizerOrganizationsAsync(@event.UserId)).ReturnsAsync(new List<Organization>() { organization });

            var cancelationToken = new CancellationToken();

            // Act
            await _organizerRightsRevokedHandler.HandleAsync(@event, cancelationToken);

            // Arrange
            _organizationRepository.Verify(repo => repo.UpdateAsync(organization), Times.Once());
            _organizerRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOrganizer_ShouldThrowOrganizerNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var @event = new OrganizerRightsRevoked(userId);
            var organizer = new Organizer(userId);
            var organization = new Organization(Guid.NewGuid(), "name", new List<Organizer>() { organizer }, new List<Organization>() { });

            _organizerRepository.Setup(repo => repo.GetAsync(@event.UserId)).ReturnsAsync((Organizer)null);
            _organizationRepository.Setup(repo => repo.GetOrganizerOrganizationsAsync(@event.UserId)).ReturnsAsync(new List<Organization>() { organization });

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _organizerRightsRevokedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().ThrowAsync<OrganizerNotFoundException>();
        }

    }
}
