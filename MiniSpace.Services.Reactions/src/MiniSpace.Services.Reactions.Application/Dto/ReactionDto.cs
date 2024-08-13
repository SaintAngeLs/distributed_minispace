using System;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
        public ReactionType Type { get; set; }
        public ReactionTargetType TargetType { get; set; } 
    }
}
