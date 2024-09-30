using System;
using System.Collections.Generic;
using System.Linq;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Comments.Application.Queries
{
    public class SearchComments : IQuery<PagedResponse<CommentDto>>
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid? ParentId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string SortBy { get; set; }  // Changed to string to handle the incoming JSON properly
        public string Direction { get; set; }

        // This property will split the SortBy string into an array
        public IEnumerable<string> SortByArray => 
            string.IsNullOrEmpty(SortBy) ? new List<string> { "CreatedAt" } : SortBy.Split(',');
    }
}
