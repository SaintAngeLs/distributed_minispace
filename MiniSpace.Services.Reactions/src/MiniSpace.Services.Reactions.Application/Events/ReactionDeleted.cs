using System.Net.Mime;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Connections;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Events
{
    public class ReactionDeleted : IEvent
    {
        public Guid StudentId {get;set;}
        public Guid Content {get;set;}
        public ReactionContentType ContentType{get;set;}
        public ReactionDeleted(Guid studentId, Guid content, ReactionContentType contentType)
        {
            StudentId=studentId;
            Content=content;
            ContentType=contentType;
        }
    }
}
