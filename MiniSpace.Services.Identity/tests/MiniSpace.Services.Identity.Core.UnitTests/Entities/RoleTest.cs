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
using Convey.CQRS.Commands;
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
            Assert.True(id1.Equals(id2));
        }

        [Fact]
        public void AggregateId_CreateWithEmptyGuid_ShouldThrowInvalidAggregateIdException() {
            Assert.Throws<InvalidAggregateIdException>(() => { var id = new AggregateId(Guid.Empty); });
        }
    }
}