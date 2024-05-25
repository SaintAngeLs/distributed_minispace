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
    public class CreateOrganizationHandlerTest
    {
        private readonly CreateOrganizationHandler _createOrganizationHandler;
        private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public CreateOrganizationHandlerTest()
        {
            _organizationRepositoryMock = new Mock<IOrganizationRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _createOrganizationHandler = new CreateOrganizationHandler(
                _organizationRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidorganizationAndAuthorised_ShouldUpdateRootOrganization()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var comand = new CreateOrganization(organizationId, "this", rootOrganizationId, parentId);

            var organization = new Organization(organizationId, "this");
            var parent = new Organization(parentId, "parent", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { parent });
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootId)).ReturnsAsync(rootOrganization);
            
            var cancelationToken = new CancellationToken();

            // Act
            await _createOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(rootOrganization), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithoutAdminRole_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var comand = new CreateOrganization(organizationId, "this", rootOrganizationId, parentId);

            var organization = new Organization(organizationId, "this");
            var parent = new Organization(parentId, "parent", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { parent });
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootId)).ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<MiniSpace.Services.Organizations.Application.Exceptions.UnauthorizedAccessException>();
        }

        [Fact]
        public async Task HandleAsync_InavalidOrganizationRoot_ShouldThrowRootOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var comand = new CreateOrganization(organizationId, "this", rootOrganizationId, parentId);

            var organization = new Organization(organizationId, "this");
            var parent = new Organization(parentId, "parent", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { });
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootId)).ReturnsAsync((Organization)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<RootOrganizationNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithoutOrganizationInOrganizationRoot_ShouldThrowParentOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var comand = new CreateOrganization(organizationId, "this", rootOrganizationId, parentId);

            var organization = new Organization(organizationId, "this");
            var parent = new Organization(parentId, "parent", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { });
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootId)).ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<ParentOrganizationNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithInvalidName_ShouldThrowInvalidOrganizationNameException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var comand = new CreateOrganization(organizationId, "", rootOrganizationId, parentId);

            var organization = new Organization(organizationId, "this");
            var parent = new Organization(parentId, "parent", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { parent });
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(comand.RootId)).ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<InvalidOrganizationNameException>();
        }
    }
}
