using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class FriendDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FriendState { get; set; }
    }
}