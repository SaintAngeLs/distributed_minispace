using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using System.Threading;
using FluentAssertions;

namespace MiniSpace.Services.Comments.Application.UnitTests.Commands.Handlers
{
    public class DeleteCommentHandlerTest
    {
        private readonly DeleteCommentHandler _deleteCommentHandler;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IAppContext> _appContextMock;

        public DeleteCommentHandlerTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _appContextMock = new Mock<IAppContext>();
            _deleteCommentHandler = new DeleteCommentHandler(
                _commentRepositoryMock.Object,
                _appContextMock.Object,
                _messageBrokerMock.Object
                );
        }
    }
}
