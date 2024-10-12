namespace MiniSpace.Services.Students.Core.Entities
{
    public class Work
    {
        public Guid OrganizationId { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public Work(Guid organizationId, string company, string position, DateTime startDate, DateTime endDate, string description)
        {
            OrganizationId = organizationId;
            Company = company;
            Position = position;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
    }
}
