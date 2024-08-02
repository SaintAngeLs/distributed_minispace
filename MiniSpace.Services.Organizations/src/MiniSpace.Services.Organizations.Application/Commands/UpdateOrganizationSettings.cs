using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class UpdateOrganizationSettings : ICommand
    {
        public Guid OrganizationId { get; }
        public OrganizationSettings Settings { get; }

        public UpdateOrganizationSettings(Guid organizationId, OrganizationSettings settings)
        {
            OrganizationId = organizationId;
            Settings = settings;
        }
    }
}
