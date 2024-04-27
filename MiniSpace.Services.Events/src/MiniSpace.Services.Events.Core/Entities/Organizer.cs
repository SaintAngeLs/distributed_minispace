using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Organizer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        
        public Organizer(Guid id, string name, string email, string organization)
        {
            Id = id;
            Name = name;
            Email = email;
            Organization = organization;
        }
    }
}