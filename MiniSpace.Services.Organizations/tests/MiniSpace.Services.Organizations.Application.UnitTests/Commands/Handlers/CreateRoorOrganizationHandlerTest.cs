using FluentAssertions;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.Commands.Handlers;
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
    public class CreateRoorOrganizationHandlerTest
    {
        private readonly CreateRootOrganizationHandler _createRootOrganizationHandler;
        private readonly Mock<IOrganizationRepository> _organizationRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public CreateRoorOrganizationHandlerTest()
        {
            _organizationRepositoryMock = new Mock<IOrganizationRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _createRootOrganizationHandler = new CreateRootOrganizationHandler(
                _organizationRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidOrganizationNameAndAuthorised_ShouldNotThrow()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var comand = new CreateRootOrganization(organizationId, "this");

            var organization = new Organization(organizationId, "this");

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            
            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createRootOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_WithoutAdminRole_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var comand = new CreateRootOrganization(organizationId, "this");

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createRootOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<MiniSpace.Services.Organizations.Application.Exceptions.UnauthorizedAccessException>();
        }

        [Fact]
        public async Task HandleAsync_InavalidOrganizationName_ShouldThrowInvalidOrganizationNameException()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var comand = new CreateRootOrganization(organizationId, "");

            var identityContext = new IdentityContext(Guid.NewGuid().ToString(), "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _createRootOrganizationHandler.HandleAsync(comand, cancelationToken);
            await act.Should().ThrowAsync<InvalidOrganizationNameException> ();
        }
    }
}
