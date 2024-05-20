using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.Models.Organizations;

namespace MiniSpace.Web.Models.Comments
{
    public class SearchCommentsModel
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid ParentId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
