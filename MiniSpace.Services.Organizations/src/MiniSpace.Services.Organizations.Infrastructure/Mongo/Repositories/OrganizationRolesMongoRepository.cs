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
    public class OrganizationRolesMongoRepository : IOrganizationRolesRepository
    {
        private readonly IMongoRepository<OrganizationRolesDocument, Guid> _rolesRepository;

        public OrganizationRolesMongoRepository(IMongoRepository<OrganizationRolesDocument, Guid> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<Role> GetRoleAsync(Guid organizationId, Guid roleId)
        {
            var rolesDocument = await _rolesRepository.GetAsync(r => r.OrganizationId == organizationId);
            return rolesDocument?.Roles.FirstOrDefault(r => r.Id == roleId)?.AsEntity();
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(Guid organizationId)
        {
            var rolesDocument = await _rolesRepository.GetAsync(r => r.OrganizationId == organizationId);
            return rolesDocument?.Roles.Select(r => r.AsEntity());
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            var rolesDocument = await _rolesRepository.FindAsync(r => r.Roles.Any(role => role.Name == roleName));
            return rolesDocument?.SelectMany(r => r.Roles).FirstOrDefault(r => r.Name == roleName)?.AsEntity();
        }

        public async Task AddRoleAsync(Role role)
        {
            var rolesDocument = await _rolesRepository.GetAsync(r => r.OrganizationId == role.Id);
            if (rolesDocument != null)
            {
                var roles = rolesDocument.Roles.ToList();
                roles.Add(role.AsDocument());
                rolesDocument.Roles = roles;
                await _rolesRepository.UpdateAsync(rolesDocument);
            }
            else
            {
                var newDocument = new OrganizationRolesDocument
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = role.Id,
                    Roles = new List<RoleEntry> { role.AsDocument() }
                };
                await _rolesRepository.AddAsync(newDocument);
            }
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var rolesDocument = await _rolesRepository.GetAsync(r => r.OrganizationId == role.Id);
            if (rolesDocument != null)
            {
                var roles = rolesDocument.Roles.ToList();
                var existingRole = roles.FirstOrDefault(r => r.Id == role.Id);
                if (existingRole != null)
                {
                    roles.Remove(existingRole);
                    roles.Add(role.AsDocument());
                    rolesDocument.Roles = roles;
                    await _rolesRepository.UpdateAsync(rolesDocument);
                }
            }
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var rolesDocument = await _rolesRepository.GetAsync(r => r.Roles.Any(role => role.Id == roleId));
            if (rolesDocument != null)
            {
                var roles = rolesDocument.Roles.ToList();
                var roleToDelete = roles.FirstOrDefault(r => r.Id == roleId);
                if (roleToDelete != null)
                {
                    roles.Remove(roleToDelete);
                    rolesDocument.Roles = roles;
                    await _rolesRepository.UpdateAsync(rolesDocument);
                }
            }
        }
    }
}
