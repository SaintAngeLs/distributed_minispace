using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Student(Guid id)
    {
        public Guid Id { get; set; } = id;
    }
}