using System;

namespace MiniSpace.Web.DTO
{
    public class EducationDto
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}