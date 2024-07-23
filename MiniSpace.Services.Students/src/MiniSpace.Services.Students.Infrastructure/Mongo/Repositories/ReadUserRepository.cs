using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    public class ReadUserRepository : IExtendedReadUserRepository, IReadUserRepository
    {
        private readonly IMongoRepository<StudentDocument, Guid> _repository;

        public ReadUserRepository(IMongoRepository<StudentDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Student> GetAsync(Guid id)
        {
            var studentDocument = await _repository.GetAsync(id);
            return studentDocument?.AsEntity();
        }

        public async Task<PagedResult<StudentDocument>> FindAsync(FilterDefinition<StudentDocument> filter, int page, int pageSize, CancellationToken cancellationToken)
        {
            var options = new FindOptions<StudentDocument, StudentDocument>
            {
                Limit = pageSize,
                Skip = (page - 1) * pageSize,
                Sort = Builders<StudentDocument>.Sort.Descending(x => x.CreatedAt),
            };

            var result = await _repository.Collection
                .FindAsync(filter, options, cancellationToken)
                .ConfigureAwait(false);

            string baseUrl = "students";

            var items = await result.ToListAsync(cancellationToken).ConfigureAwait(false);
            var totalItems = (int)await CountAsync(filter, cancellationToken).ConfigureAwait(false);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResult<StudentDocument>(await result.ToListAsync(cancellationToken).ConfigureAwait(false), page, pageSize, (int)await CountAsync(filter, cancellationToken).ConfigureAwait(false), baseUrl);
        }

        private async Task<long> CountAsync(FilterDefinition<StudentDocument> filter, CancellationToken cancellationToken)
        {
            return await _repository.Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

}
