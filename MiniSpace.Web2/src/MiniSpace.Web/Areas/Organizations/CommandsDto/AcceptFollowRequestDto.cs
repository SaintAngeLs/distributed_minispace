using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class AcceptFollowRequestDto
    {
        public Guid OrganizationId { get; set; }
        public Guid RequestId { get; set; }
    }
}