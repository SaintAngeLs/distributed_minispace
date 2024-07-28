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
    public class OrganizationMembersMongoRepository : IOrganizationMembersRepository
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;

        public OrganizationMembersMongoRepository(IMongoRepository<OrganizationDocument, Guid> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<User> GetMemberAsync(Guid organizationId, Guid memberId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            var userDocument = organization?.Users.FirstOrDefault(u => u.Id == memberId);
            return userDocument?.AsEntity();
        }

        public async Task<IEnumerable<User>> GetMembersAsync(Guid organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            return organization?.Users.Select(u => u.AsEntity());
        }

        public async Task AddMemberAsync(User member)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == member.OrganizationId);
            if (organization != null)
            {
                var users = organization.Users.ToList();
                users.Add(member.AsDocument(organization.Id));
                organization.Users = users;
                await _organizationRepository.UpdateAsync(organization);
            }
        }

        public async Task UpdateMemberAsync(User member)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == member.OrganizationId);
            if (organization != null)
            {
                var users = organization.Users.ToList();
                var userDocument = users.FirstOrDefault(u => u.Id == member.Id);
                if (userDocument != null)
                {
                    users.Remove(userDocument);
                    users.Add(member.AsDocument(organization.Id));
                    organization.Users = users;
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
        }

        public async Task DeleteMemberAsync(Guid organizationId, Guid memberId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            if (organization != null)
            {
                var users = organization.Users.ToList();
                var userDocument = users.FirstOrDefault(u => u.Id == memberId);
                if (userDocument != null)
                {
                    users.Remove(userDocument);
                    organization.Users = users;
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
        }
    }
}
