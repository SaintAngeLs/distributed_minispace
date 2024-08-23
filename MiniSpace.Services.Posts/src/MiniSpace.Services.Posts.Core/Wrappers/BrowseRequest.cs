namespace MiniSpace.Services.Posts.Core.Requests
{
    public class BrowseRequest
    {
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; } = "asc";
    }
}
