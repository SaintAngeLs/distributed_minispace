using FluentAssertions;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Application.Events.External.Handlers;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Students.Application.UnitTests.Events.External.Handlers
{
    public class SignedUpHandlerTest
    {
        private readonly SignedUpHandler _signedUpHandler;
        private const string RequiredRole = "user";
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly Mock<ILogger<SignedUpHandler>> _loggerMock;

        public SignedUpHandlerTest()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _loggerMock = new Mock<ILogger<SignedUpHandler>>();
            _signedUpHandler = new SignedUpHandler(
                _studentRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_ShouldUpdateReposytory()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new SignedUp(userId, "Adam", "Nowak", "an@email.com", RequiredRole);

            _studentRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _signedUpHandler.HandleAsync(@event, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_IvalidRole_ShouldThrowInvalidRoleException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new SignedUp(userId, "Adam", "Nowak", "an@email.com", "");

            _studentRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _signedUpHandler.HandleAsync(@event, cancelationToken);
            await act.Should().ThrowAsync<InvalidRoleException>();
        }

        [Fact]
        public async Task HandleAsync_StudentAlreadyCreated_ShouldThrowStudentAlreadyCreatedException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new SignedUp(userId, "Adam", "Nowak", "an@email.com", RequiredRole);

            _studentRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _signedUpHandler.HandleAsync(@event, cancelationToken);
            await act.Should().ThrowAsync<StudentAlreadyCreatedException>();
        }
    }
}
