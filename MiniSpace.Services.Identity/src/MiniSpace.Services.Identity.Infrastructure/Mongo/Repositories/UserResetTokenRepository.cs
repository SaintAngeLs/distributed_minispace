using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed  class UserResetTokenRepository : IUserResetTokenRepository
    {
        private readonly IMongoRepository<UserResetTokenDocument, Guid> _repository;
        

        public UserResetTokenRepository(IMongoRepository<UserResetTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task SaveAsync(UserResetToken userResetToken)
        {
            await _repository.AddAsync(userResetToken.AsDocument());
        }

        public async Task<UserResetToken> GetByUserIdAsync(Guid userId)
        {
            var document = await _repository.FindAsync(x => x.UserId == userId && x.ResetTokenExpires > DateTime.UtcNow);
            return document?.AsEntity();
        }

        public async Task InvalidateTokenAsync(Guid userId)
        {
            var document = await GetByUserIdAsync(userId);
            if (document != null)
            {
                document.ResetTokenExpires = DateTime.UtcNow;
                await _repository.UpdateAsync(document.AsDocument());
            }
        }
        public async Task<UserResetTokenDocument> GetByResetTokenAsync(string resetToken)
        {
            return await _repository.FindAsync(x => x.ResetToken == resetToken && x.ResetTokenExpires > DateTime.UtcNow);
        }
    }
}
