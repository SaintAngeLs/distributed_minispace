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
    public class OrganizationMongoRepository : IOrganizationRepository
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;
        private readonly IMongoRepository<OrganizationMembersDocument, Guid> _membersRepository;

        public OrganizationMongoRepository(
            IMongoRepository<OrganizationDocument, Guid> organizationRepository,
            IMongoRepository<OrganizationMembersDocument, Guid> membersRepository)
        {
            _organizationRepository = organizationRepository;
            _membersRepository = membersRepository;
        }

        public async Task<Organization> GetAsync(Guid id)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == id);
            return organization?.AsEntity();
        }

        public async Task<IEnumerable<Organization>> GetOrganizerOrganizationsAsync(Guid organizerId)
        {
            var memberDocuments = await _membersRepository.FindAsync(m => m.Users.Any(u => u.UserId == organizerId));
            var organizationIds = memberDocuments.Select(m => m.OrganizationId);
            var organizations = await _organizationRepository.FindAsync(o => organizationIds.Contains(o.Id));
            return organizations?.Select(o => o.AsEntity());
        }

        public Task AddAsync(Organization organization)
            => _organizationRepository.AddAsync(organization.AsDocument());

        public Task UpdateAsync(Organization organization)
            => _organizationRepository.UpdateAsync(organization.AsDocument());

        public Task DeleteAsync(Guid id)
            => _organizationRepository.DeleteAsync(id);

        public async Task<User> GetMemberAsync(Guid organizationId, Guid memberId)
        {
            var memberDocument = await _membersRepository.GetAsync(m => m.OrganizationId == organizationId);
            var userDocument = memberDocument?.Users.FirstOrDefault(u => u.UserId == memberId);
            return userDocument?.AsEntity();
        }

        public async Task<IEnumerable<User>> GetMembersAsync(Guid organizationId)
        {
            var memberDocument = await _membersRepository.GetAsync(m => m.OrganizationId == organizationId);
            return memberDocument?.Users.Select(u => u.AsEntity());
        }

        public async Task AddMemberAsync(Guid organizationId, User member)
        {
            var memberDocument = await _membersRepository.GetAsync(m => m.OrganizationId == organizationId);
            if (memberDocument != null)
            {
                var users = memberDocument.Users.ToList();
                users.Add(member.AsDocument());
                memberDocument.Users = users;
                await _membersRepository.UpdateAsync(memberDocument);
            }
            else
            {
                memberDocument = new OrganizationMembersDocument
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organizationId,
                    Users = new List<UserEntry> { member.AsDocument() }
                };
                await _membersRepository.AddAsync(memberDocument);
            }
        }

        public async Task UpdateMemberAsync(Guid organizationId, User member)
        {
            var memberDocument = await _membersRepository.GetAsync(m => m.OrganizationId == organizationId);
            if (memberDocument != null)
            {
                var users = memberDocument.Users.ToList();
                var existingMember = users.FirstOrDefault(u => u.UserId == member.Id);
                if (existingMember != null)
                {
                    users.Remove(existingMember);
                    users.Add(member.AsDocument());
                    memberDocument.Users = users;
                    await _membersRepository.UpdateAsync(memberDocument);
                }
            }
        }

        public async Task DeleteMemberAsync(Guid organizationId, Guid memberId)
        {
            var memberDocument = await _membersRepository.GetAsync(m => m.OrganizationId == organizationId);
            if (memberDocument != null)
            {
                var users = memberDocument.Users.ToList();
                var existingMember = users.FirstOrDefault(u => u.UserId == memberId);
                if (existingMember != null)
                {
                    users.Remove(existingMember);
                    memberDocument.Users = users;
                    await _membersRepository.UpdateAsync(memberDocument);
                }
            }
        }
    }
}
