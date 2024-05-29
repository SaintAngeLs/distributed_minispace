using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed class UserResetTokenRepository : IUserResetTokenRepository
    {
        private readonly IMongoRepository<UserResetTokenDocument, Guid> _repository;

        public UserResetTokenRepository(IMongoRepository<UserResetTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task SaveAsync(UserResetToken userResetToken)
        {
            if (userResetToken == null)
            {
                throw new ArgumentNullException(nameof(userResetToken));
            }

            await _repository.AddAsync(userResetToken.AsDocument());
        }
        public async Task<UserResetToken> GetByUserIdAsync(Guid userId)
        {
            var documents = await _repository.FindAsync(x => x.UserId == userId);
            var document = documents.FirstOrDefault(x => x.ResetTokenExpires > DateTime.UtcNow);
            return document?.AsEntity();
        }

        public async Task InvalidateTokenAsync(Guid userId)
        {
            var document = await GetByUserIdAsync(userId);
            if (document != null && document.ResetTokenExpires > DateTime.UtcNow)
            {
                document.ResetTokenExpires = DateTime.UtcNow; // Adjust logic as needed
                await _repository.UpdateAsync(document.AsDocument());
            }
        }

         public async Task<UserResetToken> GetByResetTokenAsync(string resetToken)
        {
            var documents = await _repository.FindAsync(x => x.ResetToken == resetToken);
            var document = documents.FirstOrDefault(x => x.ResetTokenExpires > DateTime.UtcNow);
            return document?.AsEntity(); 
        }
    }
}
