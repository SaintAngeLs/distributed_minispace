﻿using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Data.Posts
{
    public class SearchPosts
    {
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public PageableDto Pageable { get; set; }

        public SearchPosts()
        {
            Pageable = new PageableDto
            {
                Page = 1,
                Size = 10,
                Sort = new SortDto
                {
                    SortBy = new List<string> { "PublishDate" },
                    Direction = "asc"
                }
            };
        }
    }
}
