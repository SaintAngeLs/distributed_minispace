using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Core.Exceptions
{
    public class StudentAlreadyLikesCommentException : DomainException
    {
        public override string Code { get; } = "student_already_likes_comment";
        public Guid Id { get; }

        public StudentAlreadyLikesCommentException(Guid id) : base(
            $"Comment with id: {id} has already liked this comment.")
        {
            Id = id;
        }
    }
}
