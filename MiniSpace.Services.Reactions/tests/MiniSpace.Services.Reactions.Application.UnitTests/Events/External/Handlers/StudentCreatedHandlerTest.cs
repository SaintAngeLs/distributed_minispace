using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Application.Commands.Handlers;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Reactions.Application.Events.External.Handlers;
using MiniSpace.Services.Reactions.Application.Events.External;
using System.ComponentModel.Design;

namespace MiniSpace.Services.Reactions.Application.UnitTests.Events.External.Handlers
{
    public class StudentCreatedHandlerTest
    {
        private readonly StudentCreatedHandler _studentCreatedHandler;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;

        public StudentCreatedHandlerTest()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentCreatedHandler = new StudentCreatedHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var @event = new StudentCreated(studentId, "Jan Kowalski");
            var cancellationToken = new CancellationToken();

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _studentCreatedHandler.HandleAsync(@event, cancellationToken);
            await act.Should().NotThrowAsync();

        }

        [Fact]
        public async Task HandleAsync_StudentAlreadyCreated_ShuldThrowStudentAlreadyAddedException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var @event = new StudentCreated(studentId, "Jan Kowalski");
            var cancellationToken = new CancellationToken();

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _studentCreatedHandler.HandleAsync(@event, cancellationToken);
            await act.Should().ThrowAsync<StudentAlreadyAddedException>();
        }
    }
}