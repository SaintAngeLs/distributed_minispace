using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Data.Posts
{
    public class SearchPosts
    {
        public Guid StudentId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}