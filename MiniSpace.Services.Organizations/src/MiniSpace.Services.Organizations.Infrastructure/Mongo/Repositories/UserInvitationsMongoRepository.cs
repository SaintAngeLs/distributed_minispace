using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserInvitationsMongoRepository : IUserInvitationsRepository
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;

        public UserInvitationsMongoRepository(IMongoRepository<OrganizationDocument, Guid> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<Invitation> GetInvitationAsync(Guid organizationId, Guid userId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            var invitationDocument = organization?.Invitations.FirstOrDefault(i => i.UserId == userId);
            return invitationDocument?.AsEntity();
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsAsync(Guid organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            return organization?.Invitations.Select(i => i.AsEntity());
        }

        public async Task AddInvitationAsync(Invitation invitation)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == invitation.OrganizationId);
            if (organization != null)
            {
                var invitations = organization.Invitations.ToList();
                invitations.Add(invitation.AsDocument());
                organization.Invitations = invitations;
                await _organizationRepository.UpdateAsync(organization);
            }
        }

        public async Task DeleteInvitationAsync(Guid organizationId, Guid userId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            if (organization != null)
            {
                var invitations = organization.Invitations.ToList();
                var invitation = invitations.FirstOrDefault(i => i.UserId == userId);
                if (invitation != null)
                {
                    invitations.Remove(invitation);
                    organization.Invitations = invitations;
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
        }
    }
}
