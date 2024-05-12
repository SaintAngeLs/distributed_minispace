using System;

namespace MiniSpace.Services.Comments.Application.Exceptions
{
    public class StudentAlreadyExistsException : AppException
    {
        public override string Code { get; } = "student_already_added";
        public Guid StudentId { get; }
    
        public StudentAlreadyExistsException(Guid studentId)
            : base($"Student with id: {studentId} was already added.")
        {
            StudentId = studentId;
        }
    }    
}
