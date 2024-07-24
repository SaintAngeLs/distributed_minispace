using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly IMongoRepository<UserSettingsDocument, Guid> _repository;

        public UserSettingsRepository(IMongoRepository<UserSettingsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserSettings> GetUserSettingsAsync(Guid studentId)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.StudentId == studentId);
            return userSettingsDocument?.AsEntity();
        }

        public async Task AddUserSettingsAsync(UserSettings userSettings)
        {
            var userSettingsDocument = userSettings.AsDocument();
            await _repository.AddAsync(userSettingsDocument);
        }

        public async Task UpdateUserSettingsAsync(UserSettings userSettings)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.StudentId == userSettings.StudentId);

            if (userSettingsDocument == null)
            {
                userSettingsDocument = userSettings.AsDocument();
                await _repository.AddAsync(userSettingsDocument);
            }
            else
            {
                userSettingsDocument.AvailableSettings = userSettings.AvailableSettings;
                await _repository.UpdateAsync(userSettingsDocument);
            }
        }
    }
}
