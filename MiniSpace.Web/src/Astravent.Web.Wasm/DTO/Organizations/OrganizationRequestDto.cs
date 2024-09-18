using System;

namespace Astravent.Web.Wasm.DTO.Organizations
{
    public class OrganizationRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }
    }
}
