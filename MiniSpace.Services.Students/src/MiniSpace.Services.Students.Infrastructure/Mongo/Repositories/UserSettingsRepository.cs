using Convey.Persistence.MongoDB;
using MongoDB.Driver;
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
            return userSettingsDocument?.Settings ?? new UserSettings();
        }

        public async Task UpdateUserSettingsAsync(Guid studentId, UserSettings userSettings)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.StudentId == studentId);

            if (userSettingsDocument == null)
            {
                userSettingsDocument = new UserSettingsDocument
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    Settings = userSettings
                };

                await _repository.AddAsync(userSettingsDocument);
            }
            else
            {
                userSettingsDocument.Settings = userSettings;
                await _repository.UpdateAsync(userSettingsDocument);
            }
        }
    }
}
