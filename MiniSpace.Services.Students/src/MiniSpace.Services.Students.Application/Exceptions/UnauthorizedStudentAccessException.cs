using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class UnauthorizedStudentAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_student_access";
        public Guid StudentId { get; }
        public Guid UserId { get; }

        public UnauthorizedStudentAccessException(Guid studentId, Guid userId)
            : base($"Unauthorized access to student with id: '{studentId}' by user with id: '{userId}'.")
        {
            StudentId = studentId;
            UserId = userId;
        }
    }
}
