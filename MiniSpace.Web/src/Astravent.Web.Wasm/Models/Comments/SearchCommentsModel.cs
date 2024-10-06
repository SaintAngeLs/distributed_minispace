using System;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Models.Comments
{
    public class SearchCommentsModel
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid ParentId { get; set; }
        public PageableDto Pageable { get; set; }
        public bool SearchingSubmitted { get; set; }
    }
}
