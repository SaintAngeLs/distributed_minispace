using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class StudentNotFoundException : DomainException
    {
        public override string Code { get; } = "student_not_found";
        public Guid Id { get; }

        public StudentNotFoundException(Guid id) : base($"Student with id: {id} was not found.")
        {
            Id = id;
        }
    }
}
