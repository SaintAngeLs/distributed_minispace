using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class UpdateRolePermissions : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string RoleName { get; set; } 
        public string Description { get; set; } 
        public Dictionary<string, bool> Permissions { get; }

        public UpdateRolePermissions(Guid organizationId, Guid roleId, string roleName,
            string description, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            Permissions = permissions;
        }
    }
}
