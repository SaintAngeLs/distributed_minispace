using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Comments.CommandsDto
{
     public class SearchRootCommentsCommand
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public PageableDto Pageable { get; set; }

        public SearchRootCommentsCommand(Guid contextId, string commentContext, PageableDto pageable)
        {
            ContextId = contextId;
            CommentContext = commentContext;
            Pageable = pageable;
        }
    }
}