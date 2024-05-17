using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    public interface IExtendedStudentRepository : IStudentRepository
    {
       Task<PagedResult<StudentDocument>> FindAsync(FilterDefinition<StudentDocument> filter, int page, int pageSize, CancellationToken cancellationToken);
    }
}
