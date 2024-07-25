using System;
using Xunit;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Infrastructure.Services;
using MiniSpace.Services.Identity.Infrastructure.Auth;


namespace MiniSpace.Services.Identity.Infrastructure.UnitTests.Services
{
    public class TwoFactorCodeServiceTests
    {
        private readonly TwoFactorCodeService _twoFactorCodeService;

        public TwoFactorCodeServiceTests()
        {
            _twoFactorCodeService = new TwoFactorCodeService();
        }

        [Fact]
        public void ValidateCode_WithCorrectCode_ReturnsTrue()
        {
            // Arrange
            string secret = "3WFKUZ3HGQVQCXWQZI7OUHXRNTLFT5RQ";
            string correctCode = _twoFactorCodeService.GenerateCode(secret);

            // Act
            bool result = _twoFactorCodeService.ValidateCode(secret, correctCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCode_WithIncorrectCode_ReturnsFalse()
        {
            // Arrange
            string secret = "3WFKUZ3HGQVQCXWQZI7OUHXRNTLFT5RQ";
            string incorrectCode = "000000"; // Invalid code

            // Act
            bool result = _twoFactorCodeService.ValidateCode(secret, incorrectCode);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GenerateCode_GeneratesExpectedCode()
        {
            // Arrange
            string secret = "3WFKUZ3HGQVQCXWQZI7OUHXRNTLFT5RQ";
            long currentUnixTime = GetUnixTimestamp();
            string expectedCode = ComputeTotp(secret, currentUnixTime);

            // Act
            string generatedCode = _twoFactorCodeService.GenerateCode(secret);

            // Assert
            Assert.Equal(expectedCode, generatedCode);
        }

        private long GetUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private string ComputeTotp(string secret, long unixTime)
        {
            byte[] secretBytes = TwoFactorCodeService.Base32ToBytes(secret);
            int otp = TwoFactorCodeService.ComputeTotp(secretBytes, unixTime / 30);
            return otp.ToString("D6");
        }
    }
}
