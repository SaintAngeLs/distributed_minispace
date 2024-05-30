using FluentAssertions;
using MiniSpace.Services.Students.Application.Events.External.Handlers;
using MiniSpace.Services.Students.Application.Events.External;
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
    public class UserUnbannedHandlerTest
    {
        private readonly UserUnbannedHandler _userUnbannedHandler;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IEventMapper> _eventMapperMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public UserUnbannedHandlerTest()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _eventMapperMock = new Mock<IEventMapper>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _userUnbannedHandler = new UserUnbannedHandler(
                _studentRepositoryMock.Object,
                _eventMapperMock.Object,
                _messageBrokerMock.Object
            );
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_ShouldUpdateReposytory()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var student = new Student(studentId, "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new UserUnbanned(studentId);

            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _userUnbannedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_InvalidStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var student = new Student(studentId, "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new UserUnbanned(studentId);

            _studentRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _userUnbannedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().ThrowAsync<StudentNotFoundException>();
        }
    }
}
