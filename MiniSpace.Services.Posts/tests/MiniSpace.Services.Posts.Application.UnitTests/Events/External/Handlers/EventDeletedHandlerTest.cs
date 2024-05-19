using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;
using MiniSpace.Services.Posts.Application.Events.External.Handlers;
using MiniSpace.Services.Posts.Application.Events.External;
using System.ComponentModel.Design;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.UnitTests.Events.External.Handlers
{
    public class EventDeletedHandlerTest
    {
        private readonly EventDeletedHandler _eventDeletedHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ICommandDispatcher> _commandDispatcherMock;

        public EventDeletedHandlerTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _eventDeletedHandler = new EventDeletedHandler(_eventRepositoryMock.Object,
                _postRepositoryMock.Object, _commandDispatcherMock.Object);
        }

        // [Fact]
        // public async Task HandleAsync_XXX_ShouldXXX() {

        // }
    }
}