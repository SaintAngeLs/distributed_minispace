using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Comments.CommandsDto
{
    public class SearchSubCommentsCommand
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid ParentId { get; set; }
        public PageableDto Pageable { get; set; }

        public SearchSubCommentsCommand(Guid contextId, string commentContext, Guid parentId, PageableDto pageable)
        {
            ContextId = contextId;
            CommentContext = commentContext;
            ParentId = parentId;
            Pageable = pageable;
        }
    }
}