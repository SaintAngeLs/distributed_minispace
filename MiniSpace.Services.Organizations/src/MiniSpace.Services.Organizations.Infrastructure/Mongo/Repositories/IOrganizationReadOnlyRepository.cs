using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    public interface IOrganizationReadOnlyRepository
    {
        IMongoQueryable<OrganizationDocument> GetAll();
    }
}
