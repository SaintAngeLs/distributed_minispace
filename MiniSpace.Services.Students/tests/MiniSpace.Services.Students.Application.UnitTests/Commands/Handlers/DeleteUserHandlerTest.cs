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
using MiniSpace.Services.Students.Infrastructure.Contexts;

namespace MiniSpace.Services.Students.Application.UnitTests.Commands.Handlers
{
    public class DeleteUserHandlerTest
    {
        private readonly DeleteUserHandler _deleteUserHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAppContext> _appContextMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;

        public DeleteUserHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _appContextMock = new Mock<IAppContext>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _deleteUserHandler = new DeleteUserHandler(
                _userRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        [Fact]
        public async Task HandleAsync_WithValidStudentAndPermitions_ShouldDeleteStudent()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteUser(studentId);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);
            var identityContext = new IdentityContext(studentId.ToString(),
                "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act
            await _deleteUserHandler.HandleAsync(command, cancelationToken);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(command.StudentId), Times.Once);

        }


        [Fact]
        public async Task HandleAsync_UnknownStudent_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteUser(studentId);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync((Student)null);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _deleteUserHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_DeletingNotThemselfWithoutAdminRole_ShouldThrowUnauthorizedStudentAccessException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteUser(studentId);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _deleteUserHandler.HandleAsync(command, cancelationToken);
            await act.Should().ThrowAsync<AnauthorizedUserAccessException>();
        }

        [Fact]
        public async Task HandleAsync_DeletingNotThemselfWithAdminRole_ShouldNotThrowUnauthorizedStudentAccessException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteUser(studentId);

            var student = new Student(studentId, "Adam", "Nowak", "an@meail.com", DateTime.Now);
            var identityContext = new IdentityContext(Guid.NewGuid().ToString(),
                "Admin", true, new Dictionary<string, string>());

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);

            _userRepositoryMock.Setup(repo => repo.GetAsync(studentId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _deleteUserHandler.HandleAsync(command, cancelationToken);
            await act.Should().NotThrowAsync<AnauthorizedUserAccessException>();
        }
    }
}
