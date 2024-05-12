using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Participant(Guid studentId, string name)
    {
        public Guid StudentId { get; set; } = studentId;
        public string Name { get; set; } = name;
    }
}