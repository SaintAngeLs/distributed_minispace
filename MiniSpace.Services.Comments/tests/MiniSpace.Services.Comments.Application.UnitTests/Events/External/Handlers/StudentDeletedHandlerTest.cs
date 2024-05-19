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
    public class StudentDeletedHandlerTest
    {
        private readonly StudentDeletedHandler _studentDeletedHandler;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;

        public StudentDeletedHandlerTest()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentDeletedHandler = new StudentDeletedHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ValidData_ShouldNotThrowExeption()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var @event = new StudentDeleted(studentId, "Jan Kowalski");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(true);

            // Act
            await _studentDeletedHandler.HandleAsync(@event);

            // Assert
            _studentRepositoryMock.Verify(repo => repo.DeleteAsync(studentId), Times.Once());
        }

        [Fact]
        public async Task HandleAsync_StudentAlreadyDeleted_ShuldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var @event = new StudentDeleted(studentId, "Jan Kowalski");

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId))
                .ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _studentDeletedHandler.HandleAsync(@event);
            await act.Should().ThrowAsync<StudentNotFoundException>();
        }
    }
}
