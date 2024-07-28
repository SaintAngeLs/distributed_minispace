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
        private readonly IMongoRepository<OrganizationMembersDocument, Guid> _userRepository;

        public OrganizationMembersMongoRepository(IMongoRepository<OrganizationMembersDocument, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetMemberAsync(Guid organizationId, Guid memberId)
        {
            var userDocument = await _userRepository.GetAsync(u => u.OrganizationId == organizationId && u.Users.Any(m => m.Id == memberId));
            return userDocument?.Users.FirstOrDefault(u => u.Id == memberId)?.AsEntity();
        }

        public async Task<IEnumerable<User>> GetMembersAsync(Guid organizationId)
        {
            var userDocument = await _userRepository.GetAsync(u => u.OrganizationId == organizationId);
            return userDocument?.Users.Select(u => u.AsEntity());
        }

        public async Task AddMemberAsync(User member)
        {
            var userDocument = await _userRepository.GetAsync(u => u.OrganizationId == member.OrganizationId);
            if (userDocument != null)
            {
                var users = userDocument.Users.ToList();
                users.Add(member.AsDocument());
                userDocument.Users = users;
                await _userRepository.UpdateAsync(userDocument);
            }
        }

        public async Task UpdateMemberAsync(User member)
        {
            var userDocument = await _userRepository.GetAsync(u => u.OrganizationId == member.OrganizationId);
            if (userDocument != null)
            {
                var users = userDocument.Users.ToList();
                var existingMember = users.FirstOrDefault(u => u.Id == member.Id);
                if (existingMember != null)
                {
                    users.Remove(existingMember);
                    users.Add(member.AsDocument());
                    userDocument.Users = users;
                    await _userRepository.UpdateAsync(userDocument);
                }
            }
        }

        public async Task DeleteMemberAsync(Guid organizationId, Guid memberId)
        {
            var userDocument = await _userRepository.GetAsync(u => u.OrganizationId == organizationId);
            if (userDocument != null)
            {
                var users = userDocument.Users.ToList();
                var existingMember = users.FirstOrDefault(u => u.Id == memberId);
                if (existingMember != null)
                {
                    users.Remove(existingMember);
                    userDocument.Users = users;
                    await _userRepository.UpdateAsync(userDocument);
                }
            }
        }
    }
}
