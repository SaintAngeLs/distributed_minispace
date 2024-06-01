using FluentAssertions;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Students.Core.UnitTests.Entities
{
    public class AggregatedIdTest
    {
        [Fact]
        public void AggregateId_EmptyValue_ShouldThrowInvalidAggregateIdException()
        {
            // Assert & Arrange & Act
            Func<AggregateId> func = () => {return new AggregateId(Guid.Empty); };
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
