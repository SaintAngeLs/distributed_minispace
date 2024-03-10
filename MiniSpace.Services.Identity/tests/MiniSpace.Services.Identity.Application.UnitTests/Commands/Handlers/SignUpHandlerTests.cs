using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftParcel.Services.Identity.Application.Commands;
using SwiftParcel.Services.Identity.Application.Commands.Handlers;
using SwiftParcel.Services.Identity.Identity.Application.Services;

namespace SwiftParcel.Services.Identity.Application.UnitTests.Commands.Handlers
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
            var command = new SignUp(Guid.NewGuid(), "test@email.com", "password", "user", new List<string>());

            // Act
            await _signUpHandler.HandleAsync(command);

            // Assert
            _identityServiceMock.Verify(x => x.SignUpAsync(It.Is<SignUp>(cmd =>
                cmd == command
                )), Times.Once);
        }
    }
}
