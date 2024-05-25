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

namespace MiniSpace.Services.Organizations.Application.UnitTests.Commands.Handlers
{
    public class RemoveOrganizerFromOrganizationHandlerTest
    {
        private readonly RemoveOrganizerFromOrganizationHandler _removeOrganizerFromOrganizationHandler;
        private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
        private readonly Mock<IOrganizerRepository> _organizerRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public RemoveOrganizerFromOrganizationHandlerTest()
        {
            _organizationRepositoryMock = new Mock<IOrganizationRepository>();
            _organizerRepositoryMock = new Mock<IOrganizerRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _removeOrganizerFromOrganizationHandler = new RemoveOrganizerFromOrganizationHandler(
                _organizationRepositoryMock.Object,
                _organizerRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidOrganizationAndAuthorised_ShouldUpdateOrganization()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new RemoveOrganizerFromOrganization(rootOrganizationId, organizationId, organizerId);

            var organizer = new Organizer(organizerId);
            var organization = new Organization(organizationId, "this", new List<Organizer>() { organizer }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
           


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootOrganizationId)).ReturnsAsync(rootOrganization);
            _organizerRepositoryMock.Setup(repo => repo.GetAsync(comand.OrganizerId)).ReturnsAsync(organizer);

            var cancelationToken = new CancellationToken();

            // Act
            await _removeOrganizerFromOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(rootOrganization), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithoutAdminRole_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new RemoveOrganizerFromOrganization(rootOrganizationId, organizationId, organizerId);

            var organizer = new Organizer(organizerId);
            var organization = new Organization(organizationId, "this", new List<Organizer>() { organizer }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootOrganizationId)).ReturnsAsync(rootOrganization);
            _organizerRepositoryMock.Setup(repo => repo.GetAsync(comand.OrganizerId)).ReturnsAsync(organizer);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _removeOrganizerFromOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<MiniSpace.Services.Organizations.Application.Exceptions.UnauthorizedAccessException>();
        }

        [Fact]
        public async Task HandleAsync_InavalidOrganizationRoot_ShouldThrowRootOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new RemoveOrganizerFromOrganization(rootOrganizationId, organizationId, organizerId);

            var organizer = new Organizer(organizerId);
            var organization = new Organization(organizationId, "this", new List<Organizer>() { organizer }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootOrganizationId)).ReturnsAsync((Organization)null);
            _organizerRepositoryMock.Setup(repo => repo.GetAsync(comand.OrganizerId)).ReturnsAsync(organizer);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _removeOrganizerFromOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<RootOrganizationNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithoutOrganizationInOrganizationRoot_ShouldThrowOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new RemoveOrganizerFromOrganization(rootOrganizationId, organizationId, organizerId);

            var organizer = new Organizer(organizerId);
            var organization = new Organization(organizationId, "this", new List<Organizer>() { organizer }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { });
            


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootOrganizationId)).ReturnsAsync(rootOrganization);
            _organizerRepositoryMock.Setup(repo => repo.GetAsync(comand.OrganizerId)).ReturnsAsync(organizer);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _removeOrganizerFromOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<OrganizationNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidOrganizer_ShouldThrowOrganizerNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new RemoveOrganizerFromOrganization(rootOrganizationId, organizationId, organizerId);

            var organizer = new Organizer(organizerId);
            var organization = new Organization(organizationId, "this", new List<Organizer>() { organizer }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootOrganizationId)).ReturnsAsync(rootOrganization);
            _organizerRepositoryMock.Setup(repo => repo.GetAsync(comand.OrganizerId)).ReturnsAsync((Organizer)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _removeOrganizerFromOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<OrganizerNotFoundException>();
        }
    }
}
