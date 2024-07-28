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
        private readonly IMongoRepository<OrganizationInvitationDocument, Guid> _invitationRepository;

        public UserInvitationsMongoRepository(IMongoRepository<OrganizationInvitationDocument, Guid> invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task<Invitation> GetInvitationAsync(Guid organizationId, Guid userId)
        {
            var invitationDocument = await _invitationRepository.GetAsync(i => i.OrganizationId == organizationId && i.Invitations.Any(inv => inv.UserId == userId));
            return invitationDocument?.Invitations.FirstOrDefault(i => i.UserId == userId)?.AsEntity();
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsAsync(Guid organizationId)
        {
            var invitationDocument = await _invitationRepository.GetAsync(i => i.OrganizationId == organizationId);
            return invitationDocument?.Invitations.Select(i => i.AsEntity());
        }

        public async Task AddInvitationAsync(Invitation invitation)
        {
            var invitationDocument = await _invitationRepository.GetAsync(i => i.OrganizationId == invitation.OrganizationId);
            if (invitationDocument != null)
            {
                var invitations = invitationDocument.Invitations.ToList();
                invitations.Add(invitation.AsDocument());
                invitationDocument.Invitations = invitations;
                await _invitationRepository.UpdateAsync(invitationDocument);
            }
        }

        public async Task DeleteInvitationAsync(Guid organizationId, Guid userId)
        {
            var invitationDocument = await _invitationRepository.GetAsync(i => i.OrganizationId == organizationId);
            if (invitationDocument != null)
            {
                var invitations = invitationDocument.Invitations.ToList();
                var invitation = invitations.FirstOrDefault(i => i.UserId == userId);
                if (invitation != null)
                {
                    invitations.Remove(invitation);
                    invitationDocument.Invitations = invitations;
                    await _invitationRepository.UpdateAsync(invitationDocument);
                }
            }
        }
    }
}
