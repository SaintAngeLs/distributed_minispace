using System;
using System.Collections.Generic;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Core.Wrappers
{
    public class BrowseCommentsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid ContextId { get; set; }
        public CommentContext CommentContext { get; set; }
        public Guid ParentId { get; set; }
        public IEnumerable<string> SortBy { get; set; }
        public string SortDirection { get; set; }
        public BrowseCommentsRequest(int pageNumber, int pageSize, Guid contextId, 
            CommentContext context, Guid parentId, IEnumerable<string> sortBy, string sortDirection)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ContextId = contextId;
            CommentContext = context;
            ParentId = parentId;
            SortBy = sortBy;
            SortDirection = sortDirection;
        }
    }
}
