using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class UserOrganizations : AggregateRoot
    {
        public Guid UserId { get; private set; }
        private ISet<UserOrganizationEntry> _organizations = new HashSet<UserOrganizationEntry>();

        public IEnumerable<UserOrganizationEntry> Organizations
        {
            get => _organizations;
            private set => _organizations = new HashSet<UserOrganizationEntry>(value);
        }

        // Factory method to create a new UserOrganizations aggregate
        public static UserOrganizations CreateNew(Guid userId)
        {
            return new UserOrganizations(Guid.NewGuid(), userId, new HashSet<UserOrganizationEntry>());
        }

        public static UserOrganizations CreateExisting(Guid id, Guid userId, IEnumerable<UserOrganizationEntry> organizations)
        {
            return new UserOrganizations(id, userId, organizations);
        }

        private UserOrganizations(Guid id, Guid userId, IEnumerable<UserOrganizationEntry> organizations)
        {
            Id = id;
            UserId = userId;
            Organizations = organizations ?? new HashSet<UserOrganizationEntry>();
        }

        public void AddOrganization(Guid organizationId)
        {
            if (_organizations.Any(o => o.OrganizationId == organizationId))
            {
                throw new InvalidOperationException("The user is already a member of this organization.");
            }

            var newEntry = new UserOrganizationEntry(organizationId, DateTime.UtcNow);
            _organizations.Add(newEntry);
            AddEvent(new UserJoinedOrganization(UserId, organizationId, newEntry.JoinDate));
        }

        public void RemoveOrganization(Guid organizationId)
        {
            var organizationEntry = _organizations.SingleOrDefault(o => o.OrganizationId == organizationId);
            if (organizationEntry == null)
            {
                throw new InvalidOperationException("The user is not a member of this organization.");
            }

            _organizations.Remove(organizationEntry);
            AddEvent(new UserLeftOrganization(UserId, organizationId, DateTime.UtcNow));
        }

        public bool IsMemberOf(Guid organizationId)
        {
            return _organizations.Any(o => o.OrganizationId == organizationId);
        }
    }
}
