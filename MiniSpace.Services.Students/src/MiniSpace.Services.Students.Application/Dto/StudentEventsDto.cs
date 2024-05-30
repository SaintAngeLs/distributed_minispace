using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class StudentEventsDto
    {
        public Guid StudentId { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
    }
}