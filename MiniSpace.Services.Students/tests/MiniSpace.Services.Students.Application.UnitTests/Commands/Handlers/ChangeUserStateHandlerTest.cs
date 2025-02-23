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
    public class ChangeUserStateHandlerTest
    {
        private readonly ChangeUserStateHandler _changeUserStateHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEventMapper> _eventMapperMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public ChangeUserStateHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _eventMapperMock = new Mock<IEventMapper>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _changeUserStateHandler = new ChangeUserStateHandler(
                _userRepositoryMock.Object,
                _eventMapperMock.Object,
                _messageBrokerMock.Object
                );

        }

        [Fact]
        public async Task HandleAsync_WithValidStudentAndState_ShouldChangeState()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new ChangeUserState(studentId, "Valid");

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act
            await _changeUserStateHandler.HandleAsync(command, cancelationToken);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(student), Times.Once);

        }

        [Fact]
        public async Task HandleAsync_UnknownStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new ChangeUserState(studentId, "Valid");

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _changeUserStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WrongState_ShouldThrowCannotChangeStudentStateException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new ChangeUserState(studentId, "make_me_an_admin_pls");

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _changeUserStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<CannotChangeStudentStateException>();
        }

        [Fact]
        public async Task HandleAsync_AlreadyInThisState_ShouldThrowStudentStateAlreadySetException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new ChangeUserState(studentId, "Valid");

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);
            student.SetValid();

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _changeUserStateHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<StudentStateAlreadySetException>();
        }
    }
}

