using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Application.DTO
{
    public class UserPostsViewsDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<ViewDto> Views { get; set; }
    }
}
