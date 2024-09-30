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
    public class RoleTest
    {
        [Fact]
        public void IsValid_WithValidArguments_ShouldReturnTrue() {
            Assert.True(Role.IsValid("aDmIn"));
            Assert.True(Role.IsValid("ADMIN"));
            Assert.True(Role.IsValid("USER"));
            Assert.True(Role.IsValid("Banned"));
            Assert.True(Role.IsValid("BANNED"));
            Assert.True(Role.IsValid("organizer"));
            Assert.True(Role.IsValid("Organizer"));
        }

        [Fact]
        public void IsValid_WithInvalidArguments_ShouldReturnFalse() {
            Assert.False(Role.IsValid("jgvmldslm"));
        }
    }
}