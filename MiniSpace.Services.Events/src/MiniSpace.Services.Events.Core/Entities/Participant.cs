using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Participant(Guid studentId)
    {
        public Guid StudentId { get; set; } = studentId;
    }
}