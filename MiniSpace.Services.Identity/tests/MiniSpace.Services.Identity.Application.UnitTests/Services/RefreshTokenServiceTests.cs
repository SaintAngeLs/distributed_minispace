using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwiftParcel.Services.Identity.Application.Services;
using SwiftParcel.Services.Identity.Core.Entities;
using SwiftParcel.Services.Identity.Core.Repositories;
using SwiftParcel.Services.Identity.Core.Exceptions;
using SwiftParcel.Services.Identity.Application.UserDTO;
using SwiftParcel.Services.Identity.Application.Exceptions;

namespace SwiftParcel.Services.Identity.Application.UnitTests.Services
{
    public class RefreshTokenServiceTests
    {
        private readonly RefreshTokenService _refreshTokenService;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtProvider> _mockJwtProvider;
        private readonly Mock<IRgen> _mockRgen;
        public RefreshTokenServiceTests()
        {
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _mockRgen = new Mock<IRgen>();

            _refreshTokenService = new RefreshTokenService(
                _mockRefreshTokenRepository.Object,
                _mockUserRepository.Object,
                _mockJwtProvider.Object,
                _mockRgen.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ReturnsValidToken()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var expectedToken = "validToken";
            _mockRgen.Setup(x => x.Generate(30, true)).Returns(expectedToken);

            //Act
            var result = await _refreshTokenService.CreateAsync(userId);

            //Assert
            result.Should().BeEquivalentTo(expectedToken);
            _mockRefreshTokenRepository.Verify(x => x.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
        }

        [Fact]
        public async Task RevokeAsync_WithValidToken_RevokesToken()
        {
            //Arrange
            var refreshToken = "validToken";
            var token = new RefreshToken(new AggregateId(), Guid.NewGuid(), refreshToken, DateTime.UtcNow);
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(token);

            //Act
            await _refreshTokenService.RevokeAsync(refreshToken);

            //Assert
            token.Should().NotBeNull();
            token.RevokedAt.Should().NotBeNull();
            token.Revoked.Should().BeTrue();
            _mockRefreshTokenRepository.Verify(x => x.UpdateAsync(token), Times.Once);
        }

        [Fact]
        public async Task RevokeAsync_WithInvalidToken_ThrowsInvalidRefreshTokenException()
        {
            //Arrange
            var refreshToken = "invalidToken";
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(() => null);

            //Act and Assert
            await _refreshTokenService.Invoking(x => x.RevokeAsync(refreshToken)).Should().ThrowAsync<InvalidRefreshTokenException>();
        }

        [Fact]
        public async Task UseAsync_WithValidToken_ReturnsAuthDto()
        {
            //Arrange
            var refreshToken = "validToken";
            var token = new RefreshToken(new AggregateId(), Guid.NewGuid(), refreshToken, DateTime.UtcNow);
            var user = new User(Guid.NewGuid(), "test@gmail.com", "password", "user", DateTime.UtcNow, new List<string>());
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(token);
            _mockUserRepository.Setup(x => x.GetAsync(token.UserId)).ReturnsAsync(user);
            _mockJwtProvider.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IEnumerable<string>>>()))
                            .Returns(new AuthDto());

            //Act
            var result = await _refreshTokenService.UseAsync(refreshToken);

            //Assert
            result.Should().NotBeNull();
            result.RefreshToken.Should().BeEquivalentTo(refreshToken);
        }

        [Fact]
        public async Task UseAsync_WithInvalidToken_ThrowsInvalidRefreshTokenException()
        {
            //Arrange
            var refreshToken = "invalidToken";
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(() => null);

            //Act and Assert
            await _refreshTokenService.Invoking(x => x.UseAsync(refreshToken)).Should().ThrowAsync<InvalidRefreshTokenException>();
        }

        [Fact]
        public async Task UseAsync_WithRevokedToken_ThrowsRevokedRefreshTokenException()
        {
            var refreshToken = "revokedToken";
            var token = new RefreshToken(new AggregateId(), Guid.NewGuid(), refreshToken, DateTime.UtcNow, DateTime.UtcNow);
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(token);

            //Act and Assert
            await _refreshTokenService.Invoking(x => x.UseAsync(refreshToken)).Should().ThrowAsync<RevokedRefreshTokenException>();
        }

        [Fact]
        public async Task UseAsync_WithInvalidTokenUserId_ThrowsUserNotFoundException()
        {
            var refreshToken = "tokenWithInvalidUserId";
            var token = new RefreshToken(new AggregateId(), Guid.NewGuid(), refreshToken, DateTime.UtcNow);
            _mockRefreshTokenRepository.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(token);
            _mockUserRepository.Setup(x => x.GetAsync(token.UserId)).ReturnsAsync(() => null);

            //Act and Assert
            await _refreshTokenService.Invoking(x => x.UseAsync(refreshToken)).Should().ThrowAsync<UserNotFoundException>()
                .WithMessage($"User with ID: '{token.UserId}' was not found.");
        }
    }
}
