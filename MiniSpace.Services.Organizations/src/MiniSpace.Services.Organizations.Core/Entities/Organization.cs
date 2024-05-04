using MiniSpace.Services.Organizations.Core.Exceptions;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organization : AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        public string Name { get; private set; }
        public Guid ParentId { get; private set; }
        public bool IsLeaf { get; private set; }

        public IEnumerable<Organizer> Organizers
        {
            get => _organizers;
            private set => _organizers = new HashSet<Organizer>(value);
        }
        
        public Organization(Guid id, string name, Guid parentId, bool isLeaf = true, IEnumerable<Organizer> organizers = null)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            IsLeaf = isLeaf;
            Organizers = organizers ?? Enumerable.Empty<Organizer>();
        }
        
        public void RemoveOrganizer(Guid organizerId)
        {
            var organizer = _organizers.SingleOrDefault(x => x.Id == organizerId);
            if(organizer is null)
            {
                throw new OrganizerIsNotInOrganization(organizerId, Id);
            }
            _organizers.Remove(organizer);
        }

        public void AddOrganizer(Guid organizerId)
        {
            if(Organizers.Any(x => x.Id == organizerId))
            {
                throw new OrganizerAlreadyAddedToOrganizationException(organizerId, Id);
            }
            _organizers.Add(new Organizer(organizerId));
        }
        
        
        
        public void MakeParent()
            => IsLeaf = false;
    }
}