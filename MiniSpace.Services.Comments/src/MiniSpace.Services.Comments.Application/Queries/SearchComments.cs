using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Comments.Application.Queries
{
    public class SearchComments : IQuery<PagedResponse<CommentDto>>
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid? ParentId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
