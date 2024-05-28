using System;
using MiniSpace.Services.Reports.Core.Exceptions;

namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class StudentAlreadyAddedException : AppException
    {
        public override string Code { get; } = "student_already_added";
        public Guid StudentId { get; }

        public StudentAlreadyAddedException(Guid studentId)
            : base($"Student with id: {studentId} was already added.")
        {
            StudentId = studentId;
        }
    }
}