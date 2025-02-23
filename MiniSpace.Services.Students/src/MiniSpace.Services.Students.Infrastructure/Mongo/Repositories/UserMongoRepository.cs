using Paralax.Persistence.MongoDB;
using MongoDB.Driver;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserMongoRepository : IExtendeduserRepository, IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserMongoRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<User> GetAsync(Guid id)
        {
            var student = await _repository.GetAsync(o => o.Id == id);

            return student?.AsEntity();
        }

        public Task AddAsync(User student)
            => _repository.AddAsync(student.AsDocument());

        public Task UpdateAsync(User student)
            => _repository.UpdateAsync(student.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
            
        public async Task<PagedResult<UserDocument>> FindAsync(FilterDefinition<UserDocument> filter, int page, int pageSize, CancellationToken cancellationToken)
        {
            var options = new FindOptions<UserDocument, UserDocument>
            {
                Limit = pageSize,
                Skip = (page - 1) * pageSize,
                Sort = Builders<UserDocument>.Sort.Descending(x => x.CreatedAt),
            };

            var result = await _repository.Collection
                .FindAsync(filter, options, cancellationToken)
                .ConfigureAwait(false);

            string baseUrl = "students";                

            return new PagedResult<UserDocument>(await result.ToListAsync(cancellationToken).ConfigureAwait(false), page, pageSize, (int)await CountAsync(filter, cancellationToken).ConfigureAwait(false), baseUrl);
        }

         private async Task<long> CountAsync(FilterDefinition<UserDocument> filter, CancellationToken cancellationToken)
        {
            return await _repository.Collection.CountDocumentsAsync(filter).ConfigureAwait(false);
        }

        public async Task<List<User>> GetUsersByEventIdAsync(Guid eventId)
        {
            var filter = Builders<UserDocument>.Filter.Or(
                Builders<UserDocument>.Filter.AnyEq(x => x.InterestedInEvents, eventId),
                Builders<UserDocument>.Filter.AnyEq(x => x.SignedUpEvents, eventId)
            );

            // Find the matching student documents
            var studentDocuments = await _repository.Collection.Find(filter).ToListAsync();

            // Convert the documents to entities
            return studentDocuments?.ConvertAll(doc => doc.AsEntity());
        }
    }    
}
