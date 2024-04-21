using System.Reflection.Metadata;
using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class DeleteReaction : ICommand
    {
        public Guid StudentId;
        public Guid ContentId;
        public ReactionContentType ContentType;
        
        public DeleteReaction(Guid studentId, Guid contentId,
                              ReactionContentType contentType)
        {
            StudentId = studentId;
            ContentId = contentId;
            ContentType = contentType;
        }
    }    
}
