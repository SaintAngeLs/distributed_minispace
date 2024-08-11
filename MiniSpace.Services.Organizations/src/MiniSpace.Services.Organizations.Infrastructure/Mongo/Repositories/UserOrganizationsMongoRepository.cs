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
    public class UserOrganizationsMongoRepository : IUserOrganizationsRepository
    {
        private readonly IMongoRepository<UserOrganizationsDocument, Guid> _userOrganizationsRepository;

        public UserOrganizationsMongoRepository(IMongoRepository<UserOrganizationsDocument, Guid> userOrganizationsRepository)
        {
            _userOrganizationsRepository = userOrganizationsRepository;
        }

        public async Task<IEnumerable<Guid>> GetUserOrganizationsAsync(Guid userId)
        {
            var userOrganizationsDocument = await _userOrganizationsRepository.GetAsync(uo => uo.UserId == userId);
            return userOrganizationsDocument?.Organizations.Select(o => o.OrganizationId);
        }

        public async Task AddOrganizationToUserAsync(Guid userId, Guid organizationId)
        {
            var userOrganizationsDocument = await _userOrganizationsRepository.GetAsync(uo => uo.UserId == userId);
            if (userOrganizationsDocument != null)
            {
                var organizations = userOrganizationsDocument.Organizations.ToList();
                if (!organizations.Any(o => o.OrganizationId == organizationId))
                {
                    organizations.Add(new UserOrganizationEntryDocument { OrganizationId = organizationId, JoinDate = DateTime.UtcNow });
                    userOrganizationsDocument.Organizations = organizations;
                    await _userOrganizationsRepository.UpdateAsync(userOrganizationsDocument);
                }
            }
            else
            {
                var newDocument = new UserOrganizationsDocument
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Organizations = new List<UserOrganizationEntryDocument>
                    {
                        new UserOrganizationEntryDocument { OrganizationId = organizationId, JoinDate = DateTime.UtcNow }
                    }
                };
                await _userOrganizationsRepository.AddAsync(newDocument);
            }
        }

        public async Task RemoveOrganizationFromUserAsync(Guid userId, Guid organizationId)
        {
            var userOrganizationsDocument = await _userOrganizationsRepository.GetAsync(uo => uo.UserId == userId);
            if (userOrganizationsDocument != null)
            {
                var organizations = userOrganizationsDocument.Organizations.ToList();
                var organizationToRemove = organizations.FirstOrDefault(o => o.OrganizationId == organizationId);
                if (organizationToRemove != null)
                {
                    organizations.Remove(organizationToRemove);
                    userOrganizationsDocument.Organizations = organizations;
                    await _userOrganizationsRepository.UpdateAsync(userOrganizationsDocument);
                }
            }
        }

        public async Task<bool> IsUserInOrganizationAsync(Guid userId, Guid organizationId)
        {
            var userOrganizationsDocument = await _userOrganizationsRepository.GetAsync(uo => uo.UserId == userId);
            return userOrganizationsDocument?.Organizations.Any(o => o.OrganizationId == organizationId) ?? false;
        }
    }
}
