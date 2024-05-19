using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Exceptions;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Application.Commands.Handlers;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;

namespace MiniSpace.Services.Posts.Application.UnitTests.Commands.Handlers {
    public class DeletePostHandlerTest {
        private readonly DeletePostHandler _deletePostHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IMessageBroker>  _messageBrokerMock;
        private readonly Mock<IAppContext>  _appContextMock;

        public DeletePostHandlerTest() {
            _postRepositoryMock = new();
            _messageBrokerMock = new();
            _appContextMock = new();
            _deletePostHandler = new DeletePostHandler(_postRepositoryMock.Object,
            _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }

        // [Fact]
        // public async Task HandleAsync_XXX_ShouldXXX() {

        // }

        
    }
}