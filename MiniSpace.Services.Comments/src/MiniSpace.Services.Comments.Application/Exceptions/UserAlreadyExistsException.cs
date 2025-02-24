using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class UserAlreadyExistsException : AppException
    {
        public override string Code { get; } = "user_already_added";
        public Guid StudentId { get; }
    
        public UserAlreadyExistsException(Guid studentId)
            : base($"Student with id: {studentId} was already added.")
        {
            StudentId = studentId;
        }
    }    
}
