using System;

namespace MiniSpace.Services.Reports.Core.Entities
{
    public class Student(Guid id)
    {
        public Guid Id { get; private set; } = id;
    }
}