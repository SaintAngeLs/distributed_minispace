using System;
using MiniSpacePwa.DTO.Wrappers;

namespace MiniSpacePwa.Data.Posts
{
    public class SearchPosts
    {
        public Guid StudentId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}