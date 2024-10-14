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
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Posts.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Posts.Core.UnitTests.Entities {
    public class PostTest
    {
        [Fact]
        public void Create_WithWhitespace_ShouldThrowInvalidPostTextContentException() {
            // Arrange
            var id = new AggregateId();
            string textContent = " ";
            
            // Act & Assert
            Assert.Throws<InvalidPostTextContentException>(() => { 
                Post.Create(id, default, default, textContent, default, default, default, default);
                });
        }

        [Fact]
        public void Create_WithNullTextContent_ShouldThrowInvalidPostTextContentException() {
            // Arrange
            var id = new AggregateId();
            string textContent = null;
            
            // Act & Assert
            Assert.Throws<InvalidPostTextContentException>(() => { 
                Post.Create(id, default, default, textContent, default, default, default, default);
                });
        }

        [Fact]
        public void Create_WithTooLongTextContent_ShouldThrowInvalidPostTextContentException() {
            // Arrange
            var id = new AggregateId();
            string textContent = new('a', 100000);
            
            // Act & Assert
            Assert.Throws<InvalidPostTextContentException>(() => { 
                Post.Create(id, default, default, textContent, default, default, default, default);
                });
        }

        [Fact]
        public void CheckPublishDate_WithInappropriateDateTime_ShouldThrowInvalidPostPublishDateException() {
            // Arrange
            var id = new AggregateId();
            string textContent = new('a', 100);
            var post = Post.Create(id, default, default, textContent, default, DateTime.Now, default, DateTime.Now);
            
            // Act & Assert
            Assert.Throws<InvalidPostPublishDateException>(() => { 
                    post.SetToBePublished(new DateTime(2000, 1, 1, 1, 1, 1), DateTime.Now);
                });
        }

        [Fact]
        public void RemoveMediaFile_WithExisting_ShouldNotThrowException() {
            // Arrange
            var id = new AggregateId();

            // Act


            // Assert
            
        }

        [Fact]
        public void RemoveMediaFile_WithNotExisting_ShouldThrowMediaFileNotFoundException() {
            
        }
    }
}