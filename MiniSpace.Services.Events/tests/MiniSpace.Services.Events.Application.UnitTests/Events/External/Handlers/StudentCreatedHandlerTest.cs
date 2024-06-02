using MiniSpace.Services.Events.Application.Commands.Handlers;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events.External.Handlers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MiniSpace.Services.Events.Application.Events.External;
using FluentAssertions;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Application.UnitTests.Events.External.Handlers
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
        public async Task HandleAsync_WithValidArguments_ShouldUpdateRepository()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new StudentCreated(studentId, studentName);

            var student = new Student(studentId);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(false);

            var cancelationToken = new CancellationToken();

            // Act
            await _studentCreatedHandler.HandleAsync(command, cancelationToken);

            // Assert
            _studentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);
        }
        [Fact]
        public async Task HandleAsync_StudentAlredyIn_ShouldThrowStudentAlreadyAddedException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var studentName = "Adam";
            var command = new StudentCreated(studentId, studentName);

            var student = new Student(studentId);

            _studentRepositoryMock.Setup(repo => repo.ExistsAsync(studentId)).ReturnsAsync(true);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _studentCreatedHandler.HandleAsync(command, cancelationToken);

            // Assert
            await act.Should().ThrowAsync<StudentAlreadyAddedException>();
            _studentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Never);
        }
    }
}
