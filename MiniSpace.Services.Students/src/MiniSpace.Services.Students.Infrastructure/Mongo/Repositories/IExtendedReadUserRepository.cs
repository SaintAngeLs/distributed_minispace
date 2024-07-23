using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IExtendedReadUserRepository : IReadUserRepository
    {
        Task<PagedResult<StudentDocument>> FindAsync(FilterDefinition<StudentDocument> filter, int page, int pageSize, CancellationToken cancellationToken);
    }
}
