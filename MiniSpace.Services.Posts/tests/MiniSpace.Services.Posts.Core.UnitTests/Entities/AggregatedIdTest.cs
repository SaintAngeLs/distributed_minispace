using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Posts.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Posts.Core.UnitTests.Entities
{
    public class AggregatedIdTest
    {
        [Fact]
        public void AggregateId_CreatedTwice_ShouldBeDifferent()
        {
            // Arrange & Act
            var id1 = new AggregateId();
            var id2 = new AggregateId();

            // Assert
            Assert.NotEqual(id1.Value, id2.Value);
        }

        [Fact]
        public void AggregateId_CreatedTwiceSameGuid_ShouldBeSame()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var id1 = new AggregateId(id);
            var id2 = new AggregateId(id);

            // Assert
            Assert.Equal(id1.Value, id2.Value);
        }
    }
}