using System;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class InvalidStudentFullNameException : AppException
    {
        public override string Code { get; } = "invalid_student_full_name";
        public Guid StudentId { get; }
        public string StudentFullName {get;}

        public InvalidStudentFullNameException(Guid studentId, string studentFullName) :
            base($"Student with ID: '{studentId}' does not have name: {studentFullName}.")
        {
            StudentId = studentId;
            StudentFullName = studentFullName;
        }
    }
}