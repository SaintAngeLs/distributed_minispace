using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationRepository
    {
        Task<Organization> GetAsync(Guid id);
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(Guid id);
    }
}