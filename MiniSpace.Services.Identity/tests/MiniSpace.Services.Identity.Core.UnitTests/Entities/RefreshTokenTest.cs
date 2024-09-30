using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Exceptions;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Application.Commands.Handlers;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Infrastructure.Contexts;
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Identity.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Identity.Core.UnitTests.Entities
{
    public class RefreshTokenTest
    {
        [Fact]
        public void RefreshToken_WithNullToken_ShouldThrowEmptyRefreshTokenException() {
            Assert.Throws<EmptyRefreshTokenException>(() => new RefreshToken(Guid.NewGuid(),
                Guid.NewGuid(), null, DateTime.Now));
        }

        [Fact]
        public void RefreshToken_WithTokenSpacesOnly_ShouldThrowEmptyRefreshTokenException() {
            Assert.Throws<EmptyRefreshTokenException>(() => new RefreshToken(Guid.NewGuid(),
                Guid.NewGuid(), "    \t   \t   ", DateTime.Now));
        }

        [Fact]
        public void RefreshToken_WithValidParameters_ShouldNotThrowException() {
            var act = () => new RefreshToken(Guid.NewGuid(),
                Guid.NewGuid(), "valid token", DateTime.Now);
            act.Should().NotThrow();
        }

        [Fact]
        public void Revoke_WithRevoked_ShouldThrowRevokedRefreshTokenException() {
            var revokedAt = DateTime.Now;
            var token = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(),
                "token", DateTime.Now, revokedAt);
            Assert.Throws<RevokedRefreshTokenException>(() => {
                token.Revoke(DateTime.Now);
            });
        }

        [Fact]
        public void Revoke_WithNotRevoked_ShouldNotThrowException() {
            DateTime? revokedAt = null;
            var token = new RefreshToken(Guid.NewGuid(), Guid.NewGuid(),
                "token", DateTime.Now, revokedAt);
            var act = (() => {
                token.Revoke(DateTime.Now);
            });
            act.Should().NotThrow();
        }
    }
}