namespace MiniSpace.Services.Posts.Application.Dto
{
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}