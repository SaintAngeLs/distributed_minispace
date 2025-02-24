using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Posts.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserEventsDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<Guid> InterestedInEvents { get; set; }
        public IEnumerable<Guid> SignedUpEvents { get; set; }
    }
}