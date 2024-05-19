using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Comments.Application.Events;
using MiniSpace.Services.Comments.Application.Exceptions;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Application.Commands.Handlers;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using Convey.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Comments.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Comments.Core.UnitTests.Entities
{
    public class CommentTest
    {
        [Fact]
        public void Like_NotLiked_ShouldAddALike()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var likes = new HashSet<Guid>() { };
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", likes, Guid.Empty, "text",
                DateTime.Now, DateTime.Now, DateTime.Now, 0, false);

            // Act
            comment.Like(studentId);

            // Assert
            Assert.Contains(comment.Likes, id => id == studentId);
        }

        [Fact]
        public void Like_IsLiked_ShouldThrowStudentAlreadyLikesCommentException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var likes = new HashSet<Guid>() { studentId };
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", likes, Guid.Empty, "text",
                DateTime.Now, DateTime.Now, DateTime.Now, 0, false);

            // Act & Assert
            var act = new Action(() => comment.Like(studentId));
            Assert.Throws<StudentAlreadyLikesCommentException>(act);
        }

        [Fact]
        public void UnLike_IsLiked_ShouldRemoveALike()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var likes = new HashSet<Guid>() { studentId };
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", likes, Guid.Empty, "text",
                DateTime.Now, DateTime.Now, DateTime.Now, 0, false);

            // Act
            comment.UnLike(studentId);

            // Assert
            Assert.DoesNotContain(comment.Likes, id => id == studentId);
        }

        [Fact]
        public void Like_NotLiked_ShouldThrowStudentNotLikeCommentException()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var likes = new HashSet<Guid>() {  };
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", likes, Guid.Empty, "text",
                DateTime.Now, DateTime.Now, DateTime.Now, 0, false);

            // Act & Assert
            var act = new Action(() => comment.UnLike(studentId));
            Assert.Throws<StudentNotLikeCommentException>(act);
        }
        
        [Fact]
        public void Create_TooLongText_ShouldThrowInvalidCommentContentException()
        {
            // Arrange
            var commentId = new AggregateId( Guid.NewGuid());
            var text = new String('x', 301);

            // Act & Assert
            var act = new Action(() => Comment.Create(commentId, Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", Guid.Empty, text, DateTime.Now));
            Assert.Throws<InvalidCommentContentException>(act);
        }

        [Fact]
        public void Update_Empty_ShouldThrowInvalidCommentContentException()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var text = String.Empty;
            var comment = new Comment(Guid.NewGuid(), Guid.NewGuid(), CommentContext.Post,
                Guid.NewGuid(), "Adam", new HashSet<Guid>() { }, Guid.Empty, "text",
                DateTime.Now, DateTime.Now, DateTime.Now, 0, false);

            // Act & Assert
            var act = new Action(() => comment.Update(text, DateTime.Now));
            Assert.Throws<InvalidCommentContentException>(act);
        }

    }
}
