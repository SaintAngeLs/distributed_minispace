using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Comments.Application.Events.External.Handlers;
using MiniSpace.Services.Comments.Application.Events.External;
using System.ComponentModel.Design;

namespace MiniSpace.Services.Comments.Application.UnitTests.Events.External.Handlers
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

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _studentCreatedHandler.HandleAsync(@event);
            await act.Should().NotThrowAsync();

        }

        [Fact]
        public async Task HandleAsync_StudentAlreadyCreated_ShuldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var @event = new StudentCreated(studentId, "Jan Kowalski");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act & Assert
            Func<Task> act = async () => await _studentCreatedHandler.HandleAsync(@event);
            await act.Should().ThrowAsync<StudentAlreadyExistsException>();
        }
    }
}
