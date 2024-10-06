using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Organizations
{
    public class OrganizationRequestsDto
    {
        public Guid OrganizationId { get; set; }
        public IEnumerable<OrganizationRequestDto> Requests { get; set; }

        public OrganizationRequestsDto()
        {
            Requests = new List<OrganizationRequestDto>();
        }

    }
}
