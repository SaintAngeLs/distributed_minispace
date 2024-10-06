using Xunit;
using Moq;
using Paralax.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax.MessageBrokers;
using Paralax.MessageBrokers.Outbox;
using Paralax.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;
using MiniSpace.Services.Posts.Infrastructure.Services;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Services.Clients;
using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Wrappers;
using FluentAssertions;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Core.Exceptions;

namespace MiniSpace.Services.Posts.Infrastructure.UnitTests.Services
{
    public class PostServiceTest
    {
        private readonly PostsService _postService;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IStudentsServiceClient> _studentsServiceClientMock;
        private readonly Mock<IAppContext> _appContextMock;

        public PostServiceTest()
        {
            _postRepositoryMock = new();
            _studentsServiceClientMock = new();
            _appContextMock = new();
            _postService = new PostsService(
                _postRepositoryMock.Object,
                _studentsServiceClientMock.Object,
                _appContextMock.Object
            );
        }

        [Fact]
        public async void BrowsePostsAsync_WithValidParameters_ShouldReturnPagedEvents() {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new SearchPosts();
            var contextId = studentId;
            command.Pageable = new PageableDto
            {
                Page = 3,
                Size = 10,
                Sort = new SortDto()
            };
            command.Pageable.Sort.Direction = "asc";
            command.Pageable.Sort.SortBy = new List<string>();
            command.StudentId = studentId;

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            var studentEventsDto = new StudentEventsDto
            {
                InterestedInEvents = [Guid.NewGuid(), Guid.NewGuid()],
                SignedUpEvents = [Guid.NewGuid(), Guid.NewGuid()]
            };

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _studentsServiceClientMock.Setup(cl => cl.GetAsync(studentId)).ReturnsAsync(studentEventsDto);

            // Act
            Func<Task<PagedResponse<IEnumerable<PostDto>>>> act =
                async () => await _postService.BrowsePostsAsync(command);

            // Assert
            await act.Should().NotThrowAsync();
            _postRepositoryMock.Verify(repo => repo.BrowseCommentsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async void BrowsePostsAsync_WithUnauthorizedIdentity_ShouldThrowUnauthorizedPostSearchException() {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new SearchPosts();
            var contextId = studentId;
            
            do {
                contextId = Guid.NewGuid();
            } while (contextId == studentId);

            command.Pageable = new PageableDto
            {
                Page = 3,
                Size = 10,
                Sort = new SortDto()
            };
            command.Pageable.Sort.Direction = "asc";
            command.Pageable.Sort.SortBy = new List<string>();
            command.StudentId = studentId;

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            var studentEventsDto = new StudentEventsDto
            {
                InterestedInEvents = [Guid.NewGuid(), Guid.NewGuid()],
                SignedUpEvents = [Guid.NewGuid(), Guid.NewGuid()]
            };

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _studentsServiceClientMock.Setup(cl => cl.GetAsync(studentId)).ReturnsAsync(studentEventsDto);

            // Act
            Func<Task<PagedResponse<IEnumerable<PostDto>>>> act =
                async () => await _postService.BrowsePostsAsync(command);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedPostSearchException>(act);
        }

        [Fact]
        public async void BrowsePostsAsync_WithNullStudentEvents_ShouldThrowInvalidStudentServiceClientResponseException() {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new SearchPosts();
            var contextId = studentId;
            command.Pageable = new PageableDto
            {
                Page = 3,
                Size = 10,
                Sort = new SortDto()
            };
            command.Pageable.Sort.Direction = "asc";
            command.Pageable.Sort.SortBy = new List<string>();
            command.StudentId = studentId;

            var identityContext = new IdentityContext(contextId.ToString(), "", true, default);

            var studentEventsDto = new StudentEventsDto
            {
                InterestedInEvents = [Guid.NewGuid(), Guid.NewGuid()],
                SignedUpEvents = [Guid.NewGuid(), Guid.NewGuid()]
            };

            _appContextMock.Setup(ctx => ctx.Identity).Returns(identityContext);
            _studentsServiceClientMock.Setup(cl => cl.GetAsync(studentId)).ReturnsAsync((StudentEventsDto)null);

            // Act
            Func<Task<PagedResponse<IEnumerable<PostDto>>>> act =
                async () => await _postService.BrowsePostsAsync(command);

            // Assert
            await Assert.ThrowsAsync<InvalidStudentServiceClientResponseException>(act);
        }
    }
}