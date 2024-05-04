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
        public Guid Id { get; }

        public StudentNotLikeCommentException(Guid id) : base(
            $"Comment with id: {id} does not like this comment.")
        {
            Id = id;
        }
    }
}
