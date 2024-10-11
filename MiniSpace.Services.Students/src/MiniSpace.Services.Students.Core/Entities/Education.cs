namespace MiniSpace.Services.Students.Core.Entities
{
    public class Education
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid OrganizationId { get; set; }

        public Education(string institutionName, string degree, DateTime startDate, DateTime endDate, string description)
        {
            InstitutionName = institutionName;
            Degree = degree;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
    }
}
