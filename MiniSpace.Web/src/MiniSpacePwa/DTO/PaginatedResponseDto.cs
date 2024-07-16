using System;
using System.Collections.Generic;

namespace MiniSpacePwa.DTO
{
   public class PaginatedResponseDto<T>
    {
        public List<T> Results { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string NextPage { get; set; }
        public string PrevPage { get; set; }
    }

}