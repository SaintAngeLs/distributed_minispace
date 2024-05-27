
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid StudentId {get;set;}
        public string StudentFullName {get;set;}
        public Guid ContentId{get;set;}
        public ReactionContentType ContentType{get;set;}
        public ReactionType Type {get;set;}
    }    
}
