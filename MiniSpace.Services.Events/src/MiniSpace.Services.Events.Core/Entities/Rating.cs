using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Rating(Guid userId, int value)
    {
        public Guid UserId { get; set; } = userId;
        public int Value { get; set; } = value;
    }
}