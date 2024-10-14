using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class SetOrganizationVisibility : ICommand
    {
        public Guid OrganizationId { get; }
        public bool IsVisible { get; }

        public SetOrganizationVisibility(Guid organizationId, bool isVisible)
        {
            OrganizationId = organizationId;
            IsVisible = isVisible;
        }
    }
}
