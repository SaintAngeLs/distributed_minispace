using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Commands.Handlers;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Reactions.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Reactions.Core.UnitTests.Entities
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

        [Fact]
        public void Equals_WithNullOther_ShouldBeFalse() {
            var id = new AggregateId(Guid.NewGuid());
            Assert.False(id.Equals(null));
        }
    }
}