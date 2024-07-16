using System.Collections;
using System.Collections.Generic;

namespace MiniSpacePwa.Areas.Friends
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}
