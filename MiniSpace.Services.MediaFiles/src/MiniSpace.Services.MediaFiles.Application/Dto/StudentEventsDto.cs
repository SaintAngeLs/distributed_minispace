namespace MiniSpace.Services.MediaFiles.Application.Dto
{
    public class StudentEventsDto
    {
        public Guid StudentId { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
    }
}