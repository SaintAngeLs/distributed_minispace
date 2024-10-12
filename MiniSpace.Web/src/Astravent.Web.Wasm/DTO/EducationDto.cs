using System;

namespace Astravent.Web.Wasm.DTO
{
    public class EducationDto
    {
        public Guid OrganizationId { get; set; }
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; }  = null;
        public string Description { get; set; }
    }
}