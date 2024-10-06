using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Comments.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Comments.Core.UnitTests.Entities
{
    public class AggregatedIdTest
    {
        [Fact]
        public void AggregateId_EmptyValue_ShouldThrowInvalidAggregateIdException()
        {
            // Assert & Arrange & Act
            Func<AggregateId> func = () => { return new AggregateId(Guid.Empty); };
            func.Should().Throw<InvalidAggregateIdException>();
        }
        [Fact]
        public void AggregateId_CreatedTwice_ShuldBeDiffrent()
        {
            // Arrange & Act
            var id1 = new AggregateId();
            var id2 = new AggregateId();

            // Assert
            Assert.False(id1.Equals(id2));
        }

        [Fact]
        public void AggregateId_CreatedTwiceSameGuid_ShuldBeSame()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var id1 = new AggregateId(id);
            var id2 = new AggregateId(id);

            // Assert
            Assert.True(id1.Equals(id2));
        }
    }
}
