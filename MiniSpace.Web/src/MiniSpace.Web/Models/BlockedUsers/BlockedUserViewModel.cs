using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Models.BlockedUsers
{
    public class BlockedUserViewModel
    {
        public Guid BlockedUserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}