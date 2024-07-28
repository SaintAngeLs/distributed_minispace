using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class UpdateRolePermissions : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public Dictionary<string, bool> Permissions { get; }

        public UpdateRolePermissions(Guid organizationId, Guid roleId, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            Permissions = permissions;
        }
    }
}
