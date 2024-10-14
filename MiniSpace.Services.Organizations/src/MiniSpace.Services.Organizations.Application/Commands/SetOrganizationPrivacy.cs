using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class SetOrganizationPrivacy : ICommand
    {
        public Guid OrganizationId { get; }
        public bool IsPrivate { get; }

        public SetOrganizationPrivacy(Guid organizationId, bool isPrivate)
        {
            OrganizationId = organizationId;
            IsPrivate = isPrivate;
        }
    }
}
