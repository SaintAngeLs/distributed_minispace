namespace MiniSpace.Services.Students.Core.Entities
{
    public class Work
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public Work(string company, string position, DateTime startDate, DateTime endDate, string description)
        {
            Company = company;
            Position = position;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
    }
}
