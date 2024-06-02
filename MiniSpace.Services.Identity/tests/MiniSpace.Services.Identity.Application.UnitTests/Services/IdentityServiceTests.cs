using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Exceptions;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Application.Services.Identity;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Exceptions;

namespace MiniSpace.Services.Identity.Application.UnitTests.Services
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
        private readonly Mock<IUserResetTokenRepository> _mockUserResetTokenRepository;
        public IdentityServiceTests() 
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _mockMessageBroker = new Mock<IMessageBroker>();
            _mockRefreshTokenService = new Mock<IRefreshTokenService>();
            _mockLogger = new Mock<ILogger<IdentityService>>();
            _mockUserResetTokenRepository = new Mock<IUserResetTokenRepository>();

            _identityService = new IdentityService(
                _mockUserRepository.Object,
                _mockPasswordService.Object,
                _mockJwtProvider.Object,
                _mockRefreshTokenService.Object,
                _mockMessageBroker.Object,
                _mockUserResetTokenRepository.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ReturnsUserDto()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "name", "test@gmail.com", "password", "user", DateTime.UtcNow, new List<string>());
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
            var user = new User(userId, "name", command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
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
            var user = new User(userId, "name", command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
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
                "fitstName",
                "lastName",
                "validEmail@gmail.com",
                "password",
                "",
                new List<string>());
            User user = null;
            SignedUp signedUp = null;
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(() => null);
            _mockPasswordService.Setup(x => x.Hash(command.Password)).Returns("hashedPassword");
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Callback<User>(x => user = x);
            _mockMessageBroker.Setup(x => x.PublishAsync(It.IsAny<SignedUp>()))
                .Callback<IEnumerable<IEvent>>(x => signedUp = x.First() as SignedUp);

            //Act
            await _identityService.SignUpAsync(command);

            //Assert
            user.Should().NotBeNull();
            user.Role.Should().Be("user");
            user.Password.Should().NotBe(command.Password);
            user.Password.Should().Be("hashedPassword");
            signedUp.Should().NotBeNull();
            signedUp.Should().BeEquivalentTo(new SignedUp(user.Id, command.FirstName, command.LastName, user.Email, user.Role));
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _mockMessageBroker.Verify(x => x.PublishAsync(It.IsAny<SignedUp>()), Times.Once);
        }

        [Fact]
        public async Task SignUpAsync_WithInvalidEmail_ThrowsInvalidEmailException()
        {
            //Arrange
            var command = new SignUp(
                 Guid.NewGuid(),
                 "firstName",
                 "lastName",
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
                 "firstName",
                 "lastName",
                 "emailInUse@gmail.com",
                 "password",
                 "user",
                 new List<string>());
            var user = new User(Guid.NewGuid(), $"{command.FirstName} {command.LastName}", command.Email, command.Password, "user", DateTime.UtcNow, new List<string>());
            _mockUserRepository.Setup(x => x.GetAsync(command.Email)).ReturnsAsync(user);

            //Act and Assert
            await _identityService.Invoking(x => x.SignUpAsync(command)).Should().ThrowAsync<EmailInUseException>()
                .WithMessage($"Email {command.Email} is already in use.");
        }

        [Fact]
        public async Task GrantOrganizerRightsAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new GrantOrganizerRights(id);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.GrantOrganizerRightsAsync(command);
            });
        }

        [Fact]
        public async Task GrantOrganizerRightsAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new GrantOrganizerRights(id);
            var user = new User(id, "name", "email", "password", "user", DateTime.Today);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync(user);
            
            // Act & Arrange
            var act = async () => await _identityService.GrantOrganizerRightsAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.Organizer);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<OrganizerRightsGranted>()), Times.Once);
        }

        [Fact]
        public async Task RevokeOrganizerRightsAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new RevokeOrganizerRights(id);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.RevokeOrganizerRightsAsync(command);
            });
        }

        [Fact]
        public async Task RevokeOrganizerRightsAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new RevokeOrganizerRights(id);
            var user = new User(id, "name", "email", "password", "organizer", DateTime.Today);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync(user);
            
            // Act & Arrange
            var act = async () => await _identityService.RevokeOrganizerRightsAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.User);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<OrganizerRightsRevoked>()), Times.Once);
        }

        [Fact]
        public async Task BanUserAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new BanUser(id);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.BanUserAsync(command);
            });
        }

        [Fact]
        public async Task BanUserAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new BanUser(id);
            var user = new User(id, "name", "email", "password", "user", DateTime.Today);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync(user);
            
            // Act & Arrange
            var act = async () => await _identityService.BanUserAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.Banned);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<UserBanned>()), Times.Once);
        }

        [Fact]
        public async Task UnbanUserAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UnbanUser(id);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.UnbanUserAsync(command);
            });
        }

        [Fact]
        public async Task UnbanUserAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UnbanUser(id);
            var user = new User(id, "name", "email", "password", "banned", DateTime.Today);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync(user);
            
            // Act & Arrange
            var act = async () => await _identityService.UnbanUserAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.User);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<UserUnbanned>()), Times.Once);
        }

        [Fact]
        public async Task ForgotPasswordAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var email = "validEmail@gmail.com";
            var command = new ForgotPassword(id, email);
            _mockUserRepository.Setup(repo => repo.GetAsync(email)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.ForgotPasswordAsync(command);
            });
        }

        [Fact]
        public async Task ForgotPasswordAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var email = "validEmail@gmail.com";
            var command = new ForgotPassword(id, email);
            var user = new User(id, "name", email, "password", "user", DateTime.Today);
            _mockUserRepository.Setup(repo => repo.GetAsync(email)).ReturnsAsync(user);
            
            // Act & Arrange
            var act = async () => await _identityService.ForgotPasswordAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.User);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<PasswordResetTokenGenerated>()), Times.Once);
            _mockUserResetTokenRepository.Verify(repo => repo.SaveAsync(It.IsAny<UserResetToken>()));
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenUserNotInRepository_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var token = "reset token";
            var newPassword = "new secret"; 
            var userResetToken = new UserResetToken(id, token, DateTime.UtcNow.AddDays(1));
            var command = new ResetPassword(id, token, newPassword);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            _mockUserResetTokenRepository.Setup(repo => repo.GetByUserIdAsync(id)).ReturnsAsync(userResetToken);
            
            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
            { 
                await _identityService.ResetPasswordAsync(command);
            });
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenResetTokenIsInvalid_ShouldThrowInvalidTokenException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var token = "garbage";
            var newPassword = "new secret"; 
            var command = new ResetPassword(id, token, newPassword);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidTokenException>(async () => 
            { 
                await _identityService.ResetPasswordAsync(command);
            });
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenResetTokenIsNull_ShouldThrowInvalidTokenException()
        {
            // Arrange
            var id = Guid.NewGuid();
            string token = null;
            var newPassword = "new secret"; 
            var command = new ResetPassword(id, token, newPassword);
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync((User)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidTokenException>(async () => 
            { 
                await _identityService.ResetPasswordAsync(command);
            });
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenUserInRepository_ShouldUpdateRepositoryAndPublishAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var token = "reset token";
            var newPassword = "new secret"; 
            var command = new ResetPassword(id, token, newPassword);
            var user = new User(id, "name", "email", "password", "user", DateTime.Today);
            var userResetToken = new UserResetToken(id, token, DateTime.UtcNow.AddDays(1));
            _mockUserRepository.Setup(repo => repo.GetAsync(id)).ReturnsAsync(user);
            _mockUserResetTokenRepository.Setup(repo => repo.GetByUserIdAsync(id)).ReturnsAsync(userResetToken);
            
            // Act & Arrange
            var act = async () => await _identityService.ResetPasswordAsync(command);
            await act.Should().NotThrowAsync();
            Assert.True(user.Role == Role.User);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockMessageBroker.Verify(broker => broker.PublishAsync(It.IsAny<PasswordReset>()), Times.Once);
        }
    }
}
