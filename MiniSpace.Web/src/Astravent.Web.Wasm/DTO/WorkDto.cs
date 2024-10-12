using System;

namespace Astravent.Web.Wasm.DTO
{
    public class WorkDto
    {
        public Guid OrganizationId { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public string Description { get; set; }
    }
}