using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Students.Application.Commands.Handlers;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Exceptions;
namespace MiniSpace.Services.Students.Application.UnitTests.Commands.Handlers
{
    public class CompleateStudentRegistrationHandlerTest
    {
        private readonly CompleteUserRegistrationHandler _completeUserRegistrationHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEventMapper> _eventMapperMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public CompleateStudentRegistrationHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _eventMapperMock = new Mock<IEventMapper>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _completeUserRegistrationHandler = new CompleteUserRegistrationHandler(
                _userRepositoryMock.Object,
                _dateTimeProviderMock.Object,
                _eventMapperMock.Object,
                _messageBrokerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidStudentAndState_ShouldCompleateRegistration()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new CompleteUserRegistration(studentId, Guid.NewGuid(), "dec", new DateTime(2000, 1, 1), false);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);
            _dateTimeProviderMock.Setup(dtp => dtp.Now).Returns(DateTime.Now);

            var cancelationToken = new CancellationToken();

            // Act
            await _completeUserRegistrationHandler.HandleAsync(command, cancelationToken);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(student), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_UnknownStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new CompleteUserRegistration(studentId, Guid.NewGuid(), "dec", DateTime.Now, false);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _completeUserRegistrationHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_AlreadyValid_ShouldThrowStudentAlreadyRegisteredException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new CompleteUserRegistration(studentId, Guid.NewGuid(), "dec", DateTime.Now, false);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);
            student.SetValid();

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _completeUserRegistrationHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UserAlreadyRegisteredException>();
        }
    }
}
