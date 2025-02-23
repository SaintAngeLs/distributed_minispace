using FluentAssertions;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Application.Events.External.Handlers;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Exceptions;
using Xunit;

namespace MiniSpace.Services.Students.Application.UnitTests.Events.External.Handlers
{
    public class MediaFileDeletedHandlerTest
    {
        private readonly MediaFileDeletedHandler _mediaFileDeletedHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public MediaFileDeletedHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mediaFileDeletedHandler = new MediaFileDeletedHandler(_userRepositoryMock.Object);

        }

        [Fact]
        public async Task HandleAsync_ValidEvent_ShouldUpdateReposytory()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new MediaFileDeleted(mediaFileId, Guid.NewGuid(), "studentprofile");

            _userRepositoryMock.Setup(repo => repo.GetAsync(mediaFileId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancelationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HandleAsync_InvalidSource_ShouldReturn()
        {
            // Arrange
            var mediaFileId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var @event = new MediaFileDeleted(mediaFileId, Guid.NewGuid(), "");

            _userRepositoryMock.Setup(repo => repo.GetAsync(mediaFileId)).ReturnsAsync(student);

            var cancelationToken = new CancellationToken();

            // Act
            Func<Task> act = async () => await _mediaFileDeletedHandler.HandleAsync(@event, cancelationToken);

            // Assert
            await act.Should().NotThrowAsync();
            _userRepositoryMock.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Student>()), Times.Never);
        }
    }
}
