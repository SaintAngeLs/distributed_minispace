using FluentAssertions;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.Commands.Handlers;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Contexts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Organizations.Application.UnitTests.Commands.Handlers
{
    public class DeleteOrganizationHandlerTest
    {
        private readonly DeleteOrganizationHandler _deleteOrganizationHandler;
        private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public DeleteOrganizationHandlerTest()
        {
            _organizationRepositoryMock = new Mock<IOrganizationRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _deleteOrganizationHandler = new DeleteOrganizationHandler(
                _organizationRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }
        [Fact]
        public async Task HandleAsync_WithValidOrganizationAndAuthorised_ShouldDeleteChild()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new DeleteOrganization(organizationId, rootOrganizationId);

            var organization = new Organization(organizationId, "this", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            var organizer = new Organizer(organizerId);


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(rootOrganizationId))
                .ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act
            await _deleteOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Once());
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<OrganizationDeleted>()), Times.Once());
        }
        [Fact]
        public async Task HandleAsync_WithRootBeingOrganiztion_ShouldDeleteItselfFromRepo()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = rootOrganizationId;
            var organizerId = Guid.NewGuid();
            var comand = new DeleteOrganization(organizationId, rootOrganizationId);

            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() {  });
            var organizer = new Organizer(organizerId);


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(rootOrganizationId))
                .ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act
            await _deleteOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            _organizationRepositoryMock.Verify(repo => repo.DeleteAsync(rootOrganizationId), Times.Once());
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<OrganizationDeleted>()), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_WithoutBeingAdmin_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new DeleteOrganization(organizationId, rootOrganizationId);

            var organization = new Organization(organizationId, "this", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            var organizer = new Organizer(organizerId);


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(rootOrganizationId))
                .ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<Exceptions.UnauthorizedAccessException>();
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<OrganizationDeleted>()), Times.Never);

        }
        [Fact]
        public async Task HandleAsync_WithoutValidRoot_ShouldThrowRootOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new DeleteOrganization(organizationId, rootOrganizationId);

            var organization = new Organization(organizationId, "this", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { organization });
            var organizer = new Organizer(organizerId);


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(rootOrganizationId))
                .ReturnsAsync((Organization)null);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<RootOrganizationNotFoundException>();
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<OrganizationDeleted>()), Times.Never);

        }
        [Fact]
        public async Task HandleAsync_WithoutOrganizationBeingAChild_ShouldThrowOrganizationNotFoundException()
        {
            // Arrange
            var rootOrganizationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var comand = new DeleteOrganization(organizationId, rootOrganizationId);

            var organization = new Organization(organizationId, "this", new List<Organizer>() { }, new List<Organization>() { });
            var rootOrganization = new Organization(rootOrganizationId, "root", new List<Organizer>() { }, new List<Organization>() { });
            var organizer = new Organizer(organizerId);


            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _organizationRepositoryMock.Setup(repo => repo.GetAsync(rootOrganizationId))
                .ReturnsAsync(rootOrganization);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _deleteOrganizationHandler.HandleAsync(comand, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<OrganizationNotFoundException>();
            _organizationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Organization>()), Times.Never);
            _messageBrokerMock.Verify(msg => msg.PublishAsync(It.IsAny<OrganizationDeleted>()), Times.Never);

        }
    }
}
