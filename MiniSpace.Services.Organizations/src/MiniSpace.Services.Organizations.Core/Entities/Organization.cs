using MiniSpace.Services.Organizations.Core.Exceptions;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organization : AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        public string Name { get; private set; }
        public Guid ParentId { get; private set; }
        public bool IsRoot => ParentId == Guid.Empty;
        public bool IsLeaf => ParentId != Guid.Empty;

        public IEnumerable<Organizer> Organizers
        {
            get => _organizers;
            private set => _organizers = new HashSet<Organizer>(value);
        }
        
        public Organization(Guid id, string name, Guid parentId, IEnumerable<Organizer> organizers = null)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            Organizers = organizers ?? Enumerable.Empty<Organizer>();
        }
        
        public void RemoveOrganizer(Organizer organizer)
            => _organizers.Remove(organizer);

        public void AddOrganizer(Organizer organizer)
        {
            if(Organizers.Contains(organizer))
            {
                throw new OrganizerAlreadyAddedToOrganizationException(organizer.Id, Id);
            }
            _organizers.Add(organizer);
        }
    }
}