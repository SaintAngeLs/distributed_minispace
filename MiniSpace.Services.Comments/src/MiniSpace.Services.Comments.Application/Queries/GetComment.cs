using System.Diagnostics.CodeAnalysis;
using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Comments.Application.Dto;

namespace MiniSpace.Services.Comments.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetComment : IQuery<CommentDto>
    {
        public Guid CommentId { get; set; }
    }    
}
