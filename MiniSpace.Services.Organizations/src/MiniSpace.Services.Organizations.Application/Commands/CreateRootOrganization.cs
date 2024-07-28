using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Entities;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateRootOrganization : ICommand
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public OrganizationSettings Settings { get; }

        public CreateRootOrganization(Guid organizationId, string name, OrganizationSettings settings)
        {
            OrganizationId = organizationId == Guid.Empty ? Guid.NewGuid() : organizationId;
            Name = name;
            Settings = settings ?? new OrganizationSettings();
        }
    }
}
