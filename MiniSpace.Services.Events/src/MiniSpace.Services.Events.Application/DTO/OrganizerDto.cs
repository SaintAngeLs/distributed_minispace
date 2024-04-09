using System;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
    }
}