using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class FollowOrganizationDto
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
    }
}