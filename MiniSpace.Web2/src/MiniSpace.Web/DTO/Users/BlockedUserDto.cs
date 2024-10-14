using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO.Users
{
    public class BlockedUserDto
    {
        public Guid BlockerId { get; set; }
        public Guid BlockedUserId { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}