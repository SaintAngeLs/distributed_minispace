using MiniSpace.Services.Organizations.Core.Exceptions;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organization : AggregateRoot
    {
        private ISet<Organizer> _organizers = new HashSet<Organizer>();
        private ISet<Organization> _subOrganizations = new HashSet<Organization>();
        private ISet<Invitation> _invitations = new HashSet<Invitation>();
        private ISet<User> _users = new HashSet<User>();
        private ISet<Role> _roles = new HashSet<Role>();
        public string Name { get; private set; }
        public bool IsPublic { get; private set; }
        public bool IsVisible { get; private set; }
        public string Settings { get; private set; }

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

        public IEnumerable<Role> Roles
        {
            get => _roles;
            private set => _roles = new HashSet<Role>(value);
        }

        public Organization(Guid id, string name, bool isPublic, IEnumerable<Organizer> organizationOrganizers = null,
            IEnumerable<Organization> organizations = null, bool isVisible = true, string settings = null)
        {
            Id = id;
            Name = name;
            IsPublic = isPublic;
            IsVisible = isVisible;
            Settings = settings;
            Organizers = organizationOrganizers ?? Enumerable.Empty<Organizer>();
            SubOrganizations = organizations ?? Enumerable.Empty<Organization>();
            InitializeDefaultRoles();
        }

        private void InitializeDefaultRoles()
        {
            _roles.Add(new Role("Creator", GetDefaultPermissionsForCreator()));
            _roles.Add(new Role("Admin", GetDefaultPermissionsForAdmin()));
            _roles.Add(new Role("Moderator", GetDefaultPermissionsForModerator()));
            _roles.Add(new Role("User", GetDefaultPermissionsForUser()));
        }

        private Dictionary<Permission, bool> GetDefaultPermissionsForCreator()
        {
            return new Dictionary<Permission, bool>
            {
                { Permission.CreateSubGroups, true },
                { Permission.DeleteSubGroups, true },
                { Permission.EditOrganizationDetails, true },
                { Permission.InviteUsers, true },
                { Permission.RemoveMembers, true },
                { Permission.ManageMembershipRequests, true },
                { Permission.MakePosts, true },
                { Permission.EditPosts, true },
                { Permission.DeletePosts, true },
                { Permission.CommentOnPosts, true },
                { Permission.DeleteComments, true },
                { Permission.MakeReactions, true },
                { Permission.PinPosts, true },
                { Permission.CreateEvents, true },
                { Permission.EditEvents, true },
                { Permission.DeleteEvents, true },
                { Permission.ManageEventParticipation, true },
                { Permission.AssignRoles, true },
                { Permission.EditPermissions, true },
                { Permission.ViewAuditLogs, true },
                { Permission.SendMessageToAll, true },
                { Permission.CreateCommunicationChannels, true },
                { Permission.ManageFeed, true },
                { Permission.ModerateContent, true }
            };
        }

        private Dictionary<Permission, bool> GetDefaultPermissionsForAdmin()
        {
            return new Dictionary<Permission, bool>
            {
                { Permission.CreateSubGroups, true },
                { Permission.DeleteSubGroups, true },
                { Permission.EditOrganizationDetails, true },
                { Permission.InviteUsers, true },
                { Permission.RemoveMembers, true },
                { Permission.ManageMembershipRequests, true },
                { Permission.MakePosts, true },
                { Permission.EditPosts, true },
                { Permission.DeletePosts, true },
                { Permission.CommentOnPosts, true },
                { Permission.DeleteComments, true },
                { Permission.MakeReactions, true },
                { Permission.PinPosts, true },
                { Permission.CreateEvents, true },
                { Permission.EditEvents, true },
                { Permission.DeleteEvents, true },
                { Permission.ManageEventParticipation, true },
                { Permission.AssignRoles, true },
                { Permission.EditPermissions, true },
                { Permission.ViewAuditLogs, true },
                { Permission.SendMessageToAll, true },
                { Permission.CreateCommunicationChannels, true },
                { Permission.ManageFeed, true },
                { Permission.ModerateContent, true }
            };
        }

        private Dictionary<Permission, bool> GetDefaultPermissionsForModerator()
        {
            return new Dictionary<Permission, bool>
            {
                { Permission.MakePosts, true },
                { Permission.EditPosts, true },
                { Permission.DeletePosts, true },
                { Permission.CommentOnPosts, true },
                { Permission.DeleteComments, true },
                { Permission.MakeReactions, true },
                { Permission.PinPosts, true },
                { Permission.CreateEvents, true },
                { Permission.EditEvents, true },
                { Permission.DeleteEvents, true },
                { Permission.ManageEventParticipation, true },
                { Permission.ModerateContent, true }
            };
        }

        private Dictionary<Permission, bool> GetDefaultPermissionsForUser()
        {
            return new Dictionary<Permission, bool>
            {
                { Permission.MakePosts, true },
                { Permission.CommentOnPosts, true },
                { Permission.MakeReactions, true }
            };
        }

        public void InviteUser(Guid userId, string email)
        {
            if (_invitations.Any(i => i.UserId == userId))
            {
                throw new UserAlreadyInvitedException(userId, Id);
            }
            _invitations.Add(new Invitation(userId, email));
        }
        public void SetPrivacy(bool isPublic)
        {
            IsPublic = isPublic;
        }

        public void UpdateSettings(string settings)
        {
            Settings = settings;
        }

        public void SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void AssignRole(Guid memberId, string role)
        {
            var user = _users.SingleOrDefault(u => u.Id == memberId);
            if (user == null)
            {
                throw new UserNotFoundException(memberId);
            }
            _roles.Add(new Role(memberId, role));
        }

        public void UpdateRolePermissions(Guid roleId, Dictionary<Permission, bool> permissions)
        {
            var role = _roles.SingleOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }
            role.UpdatePermissions(permissions);
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
