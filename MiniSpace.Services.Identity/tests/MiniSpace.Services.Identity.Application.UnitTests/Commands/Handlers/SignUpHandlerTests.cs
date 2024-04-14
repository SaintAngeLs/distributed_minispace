using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.Commands.Handlers;
using MiniSpace.Services.Identity.Application.Services;

namespace MiniSpace.Services.Identity.Application.UnitTests.Commands.Handlers
{
    public class SignUpHandlerTests
    {
        private readonly SignUpHandler _signUpHandler;
        private readonly Mock<IIdentityService> _identityServiceMock;
        public SignUpHandlerTests()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _signUpHandler = new SignUpHandler(_identityServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_CallsSignUpAsync()
        {
            // Arrange
            var command = new SignUp(Guid.NewGuid(),  "name","test@email.com", "password", "user", new List<string>());

            // Act
            await _signUpHandler.HandleAsync(command);

            // Assert
            _identityServiceMock.Verify(x => x.SignUpAsync(It.Is<SignUp>(cmd =>
                cmd == command
                )), Times.Once);
        }
    }
}
