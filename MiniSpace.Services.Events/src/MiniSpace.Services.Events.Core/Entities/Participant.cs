using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Participant(Guid studentId, string name, string email)
    {
        public Guid StudentId { get; set; } = studentId;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
    }
}