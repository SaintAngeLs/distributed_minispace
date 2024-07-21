using MiniSpace.Services.Organizations.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organization : AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        private ISet<Organization> _subOrganizations = new HashSet<Organization>();
        private ISet<Invitation> _invitations = new HashSet<Invitation>();
        private ISet<User> _users = new HashSet<User>();
        public string Name { get; private set; }
        public bool IsPublic { get; private set; }

        public IEnumerable<Organizer> Organizers
        {
            get => _organizers;
            private set => _organizers = new HashSet<Organizer>(value);
        }

        public IEnumerable<Organization> SubOrganizations
        {
            get => _subOrganizations;
            private set => _subOrganizations = new HashSet<Organization>(value);
        }

        public IEnumerable<Invitation> Invitations
        {
            get => _invitations;
            private set => _invitations = new HashSet<Invitation>(value);
        }

        public IEnumerable<User> Users
        {
            get => _users;
            private set => _users = new HashSet<User>(value);
        }

        public Organization(Guid id, string name, bool isPublic, IEnumerable<Organizer> organizationOrganizers = null,
            IEnumerable<Organization> organizations = null)
        {
            Id = id;
            Name = name;
            IsPublic = isPublic;
            Organizers = organizationOrganizers ?? Enumerable.Empty<Organizer>();
            SubOrganizations = organizations ?? Enumerable.Empty<Organization>();
        }

        public void AddOrganizer(Guid organizerId)
        {
            if (Organizers.Any(x => x.Id == organizerId))
            {
                throw new OrganizerAlreadyAddedToOrganizationException(organizerId, Id);
            }
            _organizers.Add(new Organizer(organizerId));
        }

        public void RemoveOrganizer(Guid organizerId)
        {
            var organizer = _organizers.SingleOrDefault(x => x.Id == organizerId);
            if (organizer is null)
            {
                throw new OrganizerIsNotInOrganization(organizerId, Id);
            }
            _organizers.Remove(organizer);
        }

        public void InviteUser(Guid userId, string email)
        {
            if (_invitations.Any(i => i.UserId == userId))
            {
                throw new UserAlreadyInvitedException(userId, Id);
            }
            _invitations.Add(new Invitation(userId, email));
        }

        public void SignUpUser(Guid userId)
        {
            if (_users.Any(u => u.Id == userId))
            {
                throw new UserAlreadySignedUpException(userId, Id);
            }
            _users.Add(new User(userId));
        }

        public void SetPrivacy(bool isPublic)
        {
            IsPublic = isPublic;
        }

        public Organization GetSubOrganization(Guid id)
        {
            if (Id == id)
            {
                return this;
            }

            foreach (var subOrg in SubOrganizations)
            {
                var result = subOrg.GetSubOrganization(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public void AddSubOrganization(Organization organization)
            => _subOrganizations.Add(organization);

        public static List<Organization> FindOrganizations(Guid targetOrganizerId, Organization rootOrganization)
        {
            var organizations = new List<Organization>();
            FindOrganizationsRecursive(targetOrganizerId, rootOrganization, organizations);
            return organizations;
        }

        private static void FindOrganizationsRecursive(Guid targetOrganizerId, Organization currentOrganization,
            ICollection<Organization> organizations)
        {
            if (currentOrganization.Organizers.Any(x => x.Id == targetOrganizerId))
            {
                organizations.Add(currentOrganization);
            }

            foreach (var subOrg in currentOrganization.SubOrganizations)
            {
                FindOrganizationsRecursive(targetOrganizerId, subOrg, organizations);
            }
        }

        public static List<Guid> FindAllChildrenOrganizations(Organization rootOrganization)
        {
            var organizations = new List<Guid>();
            FindAllChildrenOrganizationsRecursive(rootOrganization, organizations);
            return organizations;
        }

        private static void FindAllChildrenOrganizationsRecursive(Organization currentOrganization,
            ICollection<Guid> organizations)
        {
            organizations.Add(currentOrganization.Id);

            foreach (var subOrg in currentOrganization.SubOrganizations)
            {
                FindAllChildrenOrganizationsRecursive(subOrg, organizations);
            }
        }

        private Organization GetParentOrganization(Guid id)
        {
            foreach (var subOrg in SubOrganizations)
            {
                if (subOrg.Id == id)
                {
                    return this;
                }

                var result = subOrg.GetParentOrganization(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public void RemoveChildOrganization(Organization organization)
        {
            var parent = GetParentOrganization(organization.Id);
            if (parent is null)
            {
                throw new ParentOfOrganizationNotFoundException(organization.Id);
            }
            parent._subOrganizations.Remove(organization);
        }
    }

  
}
