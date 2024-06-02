
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get;set; }
        public string StudentFullName { get;set; }
        public Guid ContentId { get;set; }
        public ReactionContentType ContentType { get;set; }
        public ReactionType Type { get;set; }
    }    
}
