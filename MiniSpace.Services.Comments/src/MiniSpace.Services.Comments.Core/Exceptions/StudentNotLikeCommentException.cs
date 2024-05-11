using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Core.Exceptions
{
    public class StudentNotLikeCommentException : DomainException
    {
        public override string Code { get; } = "student_not_likes_comment";
        public Guid StudentId { get; }
        public Guid CommentId { get; }

        public StudentNotLikeCommentException(Guid studentId, Guid commentId) : base(
            $"Student with id: {studentId} does not like comment with id:.")
        {
            StudentId = studentId;
            CommentId = commentId;
        }
    }
}
