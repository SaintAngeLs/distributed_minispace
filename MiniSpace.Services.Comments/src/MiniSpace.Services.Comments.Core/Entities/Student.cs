using System;

namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Student
    {
        public Guid Id { get; private set; }

        public Student(Guid id)
        {
            Id = id;
        }
    }
}