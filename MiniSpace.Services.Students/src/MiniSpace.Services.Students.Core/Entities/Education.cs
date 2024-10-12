namespace MiniSpace.Services.Students.Core.Entities
{
    public class Education
    {
        public Guid OrganizationId { get; set; }
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        
        public Education(Guid organizationId, string institutionName, string degree, DateTime startDate, DateTime endDate, string description)
        {
            OrganizationId = organizationId;
            InstitutionName = institutionName;
            Degree = degree;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
    }
}
