using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class UpdateOrganizationSettings : ICommand
    {
        public Guid OrganizationId { get; }
        public string Settings { get; }

        public UpdateOrganizationSettings(Guid organizationId, string settings)
        {
            OrganizationId = organizationId;
            Settings = settings;
        }
    }
}
