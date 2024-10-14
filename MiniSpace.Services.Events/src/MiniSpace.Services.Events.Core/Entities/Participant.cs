using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Participant(Guid userId)
    {
        public Guid UserId { get; set; } = userId;
    }
}