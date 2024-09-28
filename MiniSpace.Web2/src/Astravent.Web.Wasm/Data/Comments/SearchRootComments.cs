using System;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Data.Comments
{
    public class SearchRootComments
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchRootComments(Guid contextId, string commentContext, PageableDto pageable)
        {
            ContextId = contextId;
            CommentContext = commentContext;
            Pageable = pageable;
        }
    }
}
