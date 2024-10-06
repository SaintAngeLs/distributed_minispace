using System;

namespace MiniSpace.Web.DTO
{
    public class EducationDto
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; }  = null;
        public string Description { get; set; }
    }
}