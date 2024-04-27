using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class StudentNotFoundException : AppException
    {
        public override string Code { get; } = "student_not_found";
        public Guid StudentId { get; }

        public StudentNotFoundException(Guid studentId) : base($"Student with ID: '{studentId}' was not found.")
        {
            StudentId = studentId;
        }
    }
}