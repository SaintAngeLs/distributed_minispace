using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Rating
    {
        public Guid StudentId { get; set; }
        public int Value { get; set; }
    }
}