using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; }

        public OrganizerDto(Organizer organizer)
        {
            Id = organizer.Id;
        }
    }
}