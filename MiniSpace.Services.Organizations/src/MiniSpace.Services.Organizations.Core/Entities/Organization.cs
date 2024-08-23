using MiniSpace.Services.Organizations.Core.Exceptions;
using MiniSpace.Services.Organizations.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organization : AggregateRoot
    {
        private ISet<Organization> _subOrganizations = new HashSet<Organization>();
        private ISet<Invitation> _invitations = new HashSet<Invitation>();
        private ISet<User> _users = new HashSet<User>();
        private ISet<Role> _roles = new HashSet<Role>();
        private ISet<GalleryImage> _gallery = new HashSet<GalleryImage>();

        public string Name { get; private set; }
        public string Description { get; private set; }
        public OrganizationSettings Settings { get; private set; }
        public string BannerUrl { get; private set; }
        public string ImageUrl { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid? ParentOrganizationId { get; private set; }
        public string DefaultRoleName { get; private set; }

        public string Address { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Telephone { get; private set; }
        public string Email { get; private set; }

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

        public IEnumerable<GalleryImage> Gallery
        {
            get => _gallery;
            private set => _gallery = new HashSet<GalleryImage>(value);
        }

         public Organization(Guid id, 
                            string name, 
                            string description, 
                            OrganizationSettings settings, 
                            Guid ownerId, 
                            string bannerUrl = null, 
                            string imageUrl = null, 
                            Guid? parentOrganizationId = null, 
                            string defaultRoleName = "User", 
                            string address = null, 
                            string country = null, 
                            string city = null, 
                            string telephone = null, 
                            string email = null, 
                            IEnumerable<Organization> organizations = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Settings = settings;
            BannerUrl = bannerUrl;
            ImageUrl = imageUrl;
            OwnerId = ownerId;
            ParentOrganizationId = parentOrganizationId;
            SubOrganizations = organizations ?? Enumerable.Empty<Organization>();
            DefaultRoleName = defaultRoleName;

            Address = address;
            Country = country;
            City = city;
            Telephone = telephone;
            Email = email;

            AddEvent(new OrganizationCreated(Id, Name, Description, id, ParentOrganizationId ?? Guid.Empty, OwnerId, DateTime.UtcNow));
            InitializeDefaultRoles();
        }


        private void InitializeDefaultRoles()
        {
            _roles.Add(new Role("Creator", "Default role with all permissions for the creator.", GetDefaultPermissionsForCreator()));
            _roles.Add(new Role("Admin", "Default role with administrative permissions.", GetDefaultPermissionsForAdmin()));
            _roles.Add(new Role("Moderator", "Default role with moderation permissions.", GetDefaultPermissionsForModerator()));
            _roles.Add(new Role("User", "Default role with basic user permissions.", GetDefaultPermissionsForUser()));
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
                { Permission.ModerateContent, true },
                { Permission.ModifyGallery, true },
                { Permission.UpdateProfileImage, true },
                { Permission.UpdateOrganizationImage, true }
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
                { Permission.ModerateContent, true },
                { Permission.ModifyGallery, true },
                { Permission.UpdateProfileImage, true },
                { Permission.UpdateOrganizationImage, true }
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
                { Permission.ModerateContent, true },
                { Permission.ModifyGallery, true }
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

        public void InviteUser(Guid userId)
        {
            var user = _users.SingleOrDefault(u => u.Id == OwnerId);
            if (user == null || !user.HasPermission(Permission.InviteUsers))
            {
                throw new UnauthorizedAccessException("User does not have permission to invite users.");
            }

            if (_invitations.Any(i => i.UserId == userId))
            {
                throw new UserAlreadyInvitedException(userId, Id);
            }
            _invitations.Add(new Invitation(userId));
            AddEvent(new UserInvitedToOrganization(Id, userId, DateTime.UtcNow));
        }

        public void SetPrivacy(bool isPublic)
        {
            Settings.SetPrivacy(isPublic);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void UpdateSettings(OrganizationSettings settings)
        {
            Settings = settings;
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetVisibility(bool isVisible)
        {
            Settings.SetVisibility(isVisible);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetIsPrivate(bool isPrivate)
        {
            Settings.SetIsPrivate(isPrivate);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddComments(bool canAddComments)
        {
            Settings.SetCanAddComments(canAddComments);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddReactions(bool canAddReactions)
        {
            Settings.SetCanAddReactions(canAddReactions);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanPostPosts(bool canPostPosts)
        {
            Settings.SetCanPostPosts(canPostPosts);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanPostEvents(bool canPostEvents)
        {
            Settings.SetCanPostEvents(canPostEvents);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanMakeReposts(bool canMakeReposts)
        {
            Settings.SetCanMakeReposts(canMakeReposts);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddCommentsToPosts(bool canAddCommentsToPosts)
        {
            Settings.SetCanAddCommentsToPosts(canAddCommentsToPosts);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddReactionsToPosts(bool canAddReactionsToPosts)
        {
            Settings.SetCanAddReactionsToPosts(canAddReactionsToPosts);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddCommentsToEvents(bool canAddCommentsToEvents)
        {
            Settings.SetCanAddCommentsToEvents(canAddCommentsToEvents);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetCanAddReactionsToEvents(bool canAddReactionsToEvents)
        {
            Settings.SetCanAddReactionsToEvents(canAddReactionsToEvents);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void SetDisplayFeedInMainOrganization(bool displayFeedInMainOrganization)
        {
            Settings.SetDisplayFeedInMainOrganization(displayFeedInMainOrganization);
            AddEvent(new OrganizationSettingsUpdated(Id, Settings));
        }

        public void AssignRole(Guid memberId, string roleName)
        {
            var user = _users.SingleOrDefault(u => u.Id == memberId);
            if (user == null)
            {
                throw new UserNotFoundException(memberId);
            }
            
            var role = _roles.SingleOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                throw new RoleNotFoundException(roleName);
            }

            // Assuming User entity has a method to assign roles
            user.AssignRole(role);
            
            AddEvent(new RoleAssignedToUser(Id, memberId, roleName));
        }

        public void UpdateRolePermissions(Guid roleId, Dictionary<Permission, bool> permissions)
        {
            var role = _roles.SingleOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }
            role.UpdatePermissions(permissions);
            AddEvent(new RolePermissionsUpdated(Id, roleId, permissions));
        }

        public void UpdateRole(Guid roleId, string newName, string newDescription, Dictionary<Permission, bool> newPermissions)
        {
            var role = _roles.SingleOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }
            role.UpdateName(newName);
            role.UpdateDescription(newDescription);
            role.UpdatePermissions(newPermissions);
            AddEvent(new RoleUpdated(Id, roleId, newName, newDescription, newPermissions));
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

        public void AddRole(Role role)
        {
            if (_roles.Any(r => r.Name == role.Name))
            {
                throw new RoleAlreadyExistsException(role.Name);
            }

            _roles.Add(role);
            AddEvent(new RoleCreated(Id, role.Id, role.Name, role.Description, role.Permissions));
        }

        public void AddSubOrganization(Organization organization)
            => _subOrganizations.Add(organization);

        public void AddGalleryImage(GalleryImage image)
        {
            _gallery.Add(image);
            AddEvent(new GalleryImageAdded(Id, image.ImageId, image.ImageUrl, DateTime.UtcNow));
        }

        public void RemoveGalleryImage(Guid imageId)
        {
            var image = _gallery.SingleOrDefault(g => g.ImageId == imageId);
            if (image == null)
            {
                throw new GalleryImageNotFoundException(imageId);
            }
            _gallery.Remove(image);
            AddEvent(new GalleryImageRemoved(Id, imageId, DateTime.UtcNow));
        }

        public void UpdateBannerUrl(string bannerUrl)
        {
            BannerUrl = bannerUrl;
            AddEvent(new OrganizationBannerUrlUpdated(Id, bannerUrl, DateTime.UtcNow));
        }

        public void UpdateImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            AddEvent(new OrganizationImageUrlUpdated(Id, imageUrl, DateTime.UtcNow));
        }

        public static List<Organization> FindOrganizations(Guid targetUserId, Organization rootOrganization)
        {
            var organizations = new List<Organization>();
            FindOrganizationsRecursive(targetUserId, rootOrganization, organizations);
            return organizations;
        }

        private static void FindOrganizationsRecursive(Guid targetUserId, Organization currentOrganization,
            ICollection<Organization> organizations)
        {
            if (currentOrganization.Users.Any(x => x.Id == targetUserId))
            {
                organizations.Add(currentOrganization);
            }

            foreach (var subOrg in currentOrganization.SubOrganizations)
            {
                FindOrganizationsRecursive(targetUserId, subOrg, organizations);
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

          public void UpdateDetails(string name, 
                                  string description, 
                                  OrganizationSettings settings, 
                                  string bannerUrl, 
                                  string imageUrl, 
                                  string address = null, 
                                  string country = null, 
                                  string city = null, 
                                  string telephone = null, 
                                  string email = null)
        {
            if (name != null && Name != name)
            {
                Name = name;
                AddEvent(new OrganizationNameUpdated(Id, Name));
            }

            if (description != null && Description != description)
            {
                Description = description;
                AddEvent(new OrganizationDescriptionUpdated(Id, Description));
            }

            if (settings != null && !Settings.Equals(settings))
            {
                Settings = settings;
                AddEvent(new OrganizationSettingsUpdated(Id, Settings));
            }

            if (bannerUrl != null && BannerUrl != bannerUrl)
            {
                BannerUrl = bannerUrl;
                AddEvent(new OrganizationBannerUrlUpdated(Id, BannerUrl, DateTime.UtcNow));
            }

            if (imageUrl != null && ImageUrl != imageUrl)
            {
                ImageUrl = imageUrl;
                AddEvent(new OrganizationImageUrlUpdated(Id, ImageUrl, DateTime.UtcNow));
            }

            // Update new properties
            if (address != null && Address != address)
            {
                Address = address;
            }

            if (country != null && Country != country)
            {
                Country = country;
            }

            if (city != null && City != city)
            {
                City = city;
            }

            if (telephone != null && Telephone != telephone)
            {
                Telephone = telephone;
            }

            if (email != null && Email != email)
            {
                Email = email;
            }
        }

        public void SetProfileImage(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public void SetBannerImage(string imageUrl)
        {
            BannerUrl = imageUrl;
        }

        public void UpdateDefaultRole(string roleName)
        {
            DefaultRoleName = roleName;
        }
    }
}
