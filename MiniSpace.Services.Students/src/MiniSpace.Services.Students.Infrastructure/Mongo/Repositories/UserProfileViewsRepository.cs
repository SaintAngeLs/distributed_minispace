using System;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using Convey.Persistence.MongoDB;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    public class UserProfileViewsRepository : IUserProfileViewsRepository
    {
        private readonly IMongoRepository<UserProfileViewsDocument, Guid> _repository;

        public UserProfileViewsRepository(IMongoRepository<UserProfileViewsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserProfileViews> GetAsync(Guid userId)
        {
            var document = await _repository.GetAsync(x => x.UserId == userId);
            return document?.ToEntity();
        }

        public async Task AddAsync(UserProfileViews userProfileViews)
        {
            var document = userProfileViews.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(UserProfileViews userProfileViews)
        {
            var document = userProfileViews.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(x => x.UserId == userId);
        }
    }
}
