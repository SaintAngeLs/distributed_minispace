using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey.Auth;
using Microsoft.Extensions.Logging;
using SwiftParcel.Services.Identity.Application.Commands;
using SwiftParcel.Services.Identity.Application.Events;
using SwiftParcel.Services.Identity.Application.Services;
using SwiftParcel.Services.Identity.Application.UserDTO;
using SwiftParcel.Services.Identity.Core.Entities;
using SwiftParcel.Services.Identity.Core.Exceptions;
using SwiftParcel.Services.Identity.Core.Repositories;
using SwiftParcel.Services.Identity.Identity.Application.Services;
using SwiftParcel.Services.Identity.Identity.Application.UserDTO;

namespace SwiftParcel.Services.Identity.Application.UnitTests.Services
{
    public class IdentityServiceTests
    {
        private readonly IdentityService _identityService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IJwtProvider> _mockJwtProvider;
        private readonly Mock<IMessageBroker> _mockMessageBroker;
        private readonly Mock<IRefreshTokenService> _mockRefreshTokenService;
        private readonly Mock<ILogger<IdentityService>> _mockLogger;
        public IdentityServiceTests() 
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _mockMessageBroker = new Mock<IMessageBroker>();
            _mockRefreshTokenService = new Mock<IRefreshTokenService>();
            _mockLogger = new Mock<ILogger<IdentityService>>();

            _identityService = new IdentityService(
                _mockUserRepository.Object,
                _mockPasswordService.Object,
                _mockJwtProvider.Object,
                _mockRefreshTokenService.Object,
                _mockMessageBroker.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ReturnsUserDto()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "test@gmail.com", "password", "user", DateTime.UtcNow, new List<string>());
            _mockUserRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(user);

            //Act
            var result = await _identityService.GetAsync(userId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result.Should().BeEquivalentTo(new UserDto(user));
        }

        [Fact]
        public async Task GetAsync_WithInvalidId_ReturnsNull()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(() => null);

            //Act
            var result = await _identityService.GetAsync(userId);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SignInAsync_WithValidCredentials_ReturnsAuthDto()
        {
            //Arrange
            var command = new SignIn("validEmail@gmail.com", "password");
            var userId = Guid.NewGuid();
            var user = new User(userId, command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
            var authDto = new AuthDto();
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(user);
            _mockPasswordService.Setup(x => x.IsValid(user.Password, command.Password)).Returns(true);
            _mockJwtProvider.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IEnumerable<string>>>()))
                                        .Returns(authDto);
            _mockRefreshTokenService.Setup(x => x.CreateAsync(user.Id)).ReturnsAsync("refreshToken");

            //Act
            var result = await _identityService.SignInAsync(command);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AuthDto>();
            result.Should().BeEquivalentTo(authDto);
            result.RefreshToken.Should().Be("refreshToken");
            _mockMessageBroker.Verify(x => x.PublishAsync(It.IsAny<SignedIn>()), Times.Once);
        }

        [Fact] 
        public async Task SignInAsync_WithInvalidEmail_ThrowsInvalidEmailException()
        {
            //Arrange
            var command = new SignIn("invalidEmail", "password");

            //Act and Assert
            await _identityService.Invoking(x => x.SignInAsync(command)).Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Invalid email: {command.Email}.");
        }

        [Fact]
        public async Task SignInAsync_UserNotFound_ThrowsInvalidCredentialsException()
        {
            //Arrange
            var command = new SignIn("validEmail@gmail.com", "password");
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(() => null);

            //Act and Assert
            await _identityService.Invoking(x => x.SignInAsync(command)).Should().ThrowAsync<InvalidCredentialsException>()
                .WithMessage($"Invalid credentials.");
        }

        [Fact]
        public async Task SignInAsync_WithInvalidPassword_ThrowsInvalidCredentialsException()
        {
            //Arrange
            var command = new SignIn("validEmail@gmail.com", "invalidPassword");
            var userId = Guid.NewGuid();
            var user = new User(userId, command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(user);
            _mockPasswordService.Setup(x => x.IsValid(user.Password, command.Password)).Returns(false);

            //Act and Assert
            await _identityService.Invoking(x => x.SignInAsync(command)).Should().ThrowAsync<InvalidCredentialsException>()
                .WithMessage($"Invalid credentials.");
        }

        [Fact]
        public async Task SignUpAsync_WithValidCredentials_CreatesUser()
        {
            //Arrange
            var command = new SignUp(
                Guid.NewGuid(),
                "validEmail@gmail.com",
                "password",
                "",
                new List<string>());
            User user = null;
            SignedUp signedUp = null;
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(() => null);
            _mockPasswordService.Setup(x => x.Hash(command.Password)).Returns("hashedPassword");
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Callback<User>(x => user = x);
            _mockMessageBroker.Setup(x => x.PublishAsync(It.IsAny<SignedUp>())).Callback<SignedUp>(x => signedUp = x);

            //Act
            await _identityService.SignUpAsync(command);

            //Assert
            user.Should().NotBeNull();
            user.Role.Should().Be("user");
            user.Password.Should().NotBe(command.Password);
            user.Password.Should().Be("hashedPassword");
            signedUp.Should().NotBeNull();
            signedUp.Should().BeEquivalentTo(new SignedUp(user.Id, user.Email, user.Role));
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _mockMessageBroker.Verify(x => x.PublishAsync(It.IsAny<SignedUp>()), Times.Once);
        }

        [Fact]
        public async Task SignUpAsync_WithInvalidEmail_ThrowsInvalidEmailException()
        {
            //Arrange
            var command = new SignUp(
                 Guid.NewGuid(),
                 "invalidEmail",
                 "password",
                 "user",
                 new List<string>());

            //Act and Assert
            await _identityService.Invoking(x => x.SignUpAsync(command)).Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Invalid email: {command.Email}.");
        }

        [Fact]
        public async Task SignUpAsync_WhenEmailInUse_ThrowsEmailInUseException()
        {
            //Arrange
            var command = new SignUp(
                 Guid.NewGuid(),
                 "emailInUse@gmail.com",
                 "password",
                 "user",
                 new List<string>());
            var user = new User(Guid.NewGuid(), command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(user);

            //Act and Assert
            await _identityService.Invoking(x => x.SignUpAsync(command)).Should().ThrowAsync<EmailInUseException>()
                .WithMessage($"Email {command.Email} is already in use.");
        }
    }
}
