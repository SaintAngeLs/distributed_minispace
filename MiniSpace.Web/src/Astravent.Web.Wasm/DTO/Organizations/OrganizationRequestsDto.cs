using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Organizations
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
