using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Data.Comments
{
    public class SearchSubComments
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid ParentId { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchSubComments(Guid contextId, string commentContext, Guid parentId, PageableDto pageable)
        {
            ContextId = contextId;
            CommentContext = commentContext;
            ParentId = parentId;
            Pageable = pageable;
        }
    }
}
