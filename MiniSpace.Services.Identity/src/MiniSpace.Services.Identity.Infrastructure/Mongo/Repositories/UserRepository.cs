using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    internal sealed  class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;
        private readonly UserResetTokenRepository _userResetTokenRepository;

         public UserRepository(IMongoRepository<UserDocument, Guid> repository, UserResetTokenRepository userResetTokenRepository)
        {
            _repository = repository;
            _userResetTokenRepository = userResetTokenRepository;
        }
        public async Task<User> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(id);

            return user?.AsEntity();
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsEntity();
        }

        public Task AddAsync(User user) => _repository.AddAsync(user.AsDocument());
        public Task UpdateAsync(User user) => _repository.UpdateAsync(user.AsDocument());

        public async Task<User> GetByResetTokenAsync(string resetToken)
        {
            var tokenDocument = await _userResetTokenRepository.GetByResetTokenAsync(resetToken);
            if (tokenDocument == null || tokenDocument.ResetTokenExpires <= DateTime.UtcNow)
            {
                return null;
            }
            var userDocument = await _repository.GetAsync(tokenDocument.UserId);
            return userDocument?.AsEntity();
        }

    }
}