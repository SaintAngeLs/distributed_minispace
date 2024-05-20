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
using MiniSpace.Services.Posts.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Posts.Core.UnitTests.Entities {
    public class PostTest
    {
        // TODO: prevent exception leakage

        // [Fact]
        // public void Update_WithWhitespace_ShouldThrowInvalidPostTextContentException() {
        //     // Arrange
        //     var id = new AggregateId();
        //     string textContent = " ";
        //     var post = Post.Create(id, default, default, default, default, default, default, default);
        //     // Act & Assert
        //     Action act = () => { post.Update(textContent, "a", default); };
        //     var ex = Record.Exception(act);
        //     Assert.NotNull(ex);
        //     Assert.IsType<InvalidPostTextContentException>(ex);
        // }

        // [Fact]
        // public void Update_WithNullTextContent_ShouldThrowInvalidPostTextContentException() {
        //     // Arrange
        //     var id = new AggregateId();
        //     string textContent = null;
        //     var post = Post.Create(id, default, default, default, default, default, default, default);
        //     // Act & Assert
        //     Action act = () => { post.Update(textContent, "a", default); };
        //     var ex = Record.Exception(act);
        //     Assert.NotNull(ex);
        //     Assert.IsType<InvalidPostTextContentException>(ex);
        // }

        // [Fact]
        // public void Update_WithTooLongTextContent_ShouldThrowInvalidPostTextContentException() {
        //     // Arrange
        //     var id = new AggregateId();
        //     string textContent = new('a', 100000);
        //     var post = Post.Create(id, default, default, default, default, default, default, default);
        //     // Act & Assert
        //     Action act = () => { post.Update(textContent, "a", default); };
        //     var ex = Record.Exception(act);
        //     Assert.NotNull(ex);
        //     Assert.IsType<InvalidPostTextContentException>(ex);
        // }
    }
}