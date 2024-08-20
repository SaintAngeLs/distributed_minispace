using System;
using System.Collections.Generic;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Core.Wrappers
{
    public class BrowseCommentsRequest
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public Guid ContextId { get; }
        public CommentContext Context { get; }
        public Guid ParentId { get; }
        public IEnumerable<string> SortBy { get; }
        public string SortDirection { get; }

        public BrowseCommentsRequest(int pageNumber, int pageSize, Guid contextId, 
            CommentContext context, Guid parentId, IEnumerable<string> sortBy, string sortDirection)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ContextId = contextId;
            Context = context;
            ParentId = parentId;
            SortBy = sortBy;
            SortDirection = sortDirection;
        }
    }
}
