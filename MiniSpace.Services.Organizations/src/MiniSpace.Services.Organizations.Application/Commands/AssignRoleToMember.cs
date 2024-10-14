using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AssignRoleToMember : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid MemberId { get; }
        public string Role { get; }

        public AssignRoleToMember(Guid organizationId, Guid memberId, string role)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
            Role = role;
        }
    }
}
