using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Rating(Guid studentId, int value)
    {
        public Guid StudentId { get; set; } = studentId;
        public int Value { get; set; } = value;
    }
}