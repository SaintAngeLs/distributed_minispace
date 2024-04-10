using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Student(Guid id, string name)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
    }
}