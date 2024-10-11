using Paralax.Persistence.MongoDB;
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
            var userSettingsDocument = await _repository.GetAsync(x => x.UserId == studentId);
            return userSettingsDocument?.AsEntity();
        }

        public async Task AddUserSettingsAsync(UserSettings userSettings)
        {
            var userSettingsDocument = userSettings.AsDocument();
            await _repository.AddAsync(userSettingsDocument);
        }

        public async Task UpdateUserSettingsAsync(UserSettings userSettings)
        {
            var userSettingsDocument = await _repository.GetAsync(x => x.UserId == userSettings.UserId);

            if (userSettingsDocument == null)
            {
                userSettingsDocument = userSettings.AsDocument();
                await _repository.AddAsync(userSettingsDocument);
            }
            else
            {
                userSettingsDocument.AvailableSettings = new UserAvailableSettingsDocument
                {
                    CreatedAtVisibility = userSettings.AvailableSettings.CreatedAtVisibility,
                    DateOfBirthVisibility = userSettings.AvailableSettings.DateOfBirthVisibility,
                    InterestedInEventsVisibility = userSettings.AvailableSettings.InterestedInEventsVisibility,
                    SignedUpEventsVisibility = userSettings.AvailableSettings.SignedUpEventsVisibility,
                    EducationVisibility = userSettings.AvailableSettings.EducationVisibility,
                    WorkPositionVisibility = userSettings.AvailableSettings.WorkPositionVisibility,
                    LanguagesVisibility = userSettings.AvailableSettings.LanguagesVisibility,
                    InterestsVisibility = userSettings.AvailableSettings.InterestsVisibility,
                    ContactEmailVisibility = userSettings.AvailableSettings.ContactEmailVisibility,
                    PhoneNumberVisibility = userSettings.AvailableSettings.PhoneNumberVisibility,
                    ProfileImageVisibility = userSettings.AvailableSettings.ProfileImageVisibility,
                    BannerImageVisibility = userSettings.AvailableSettings.BannerImageVisibility,
                    GalleryVisibility = userSettings.AvailableSettings.GalleryVisibility,

                    ConnectionVisibility = userSettings.AvailableSettings.ConnectionVisibility,
                    FollowersVisibility = userSettings.AvailableSettings.FollowersVisibility,
                    FollowingVisibility = userSettings.AvailableSettings.FollowingVisibility,
                    MyPostsVisibility = userSettings.AvailableSettings.MyPostsVisibility,
                    ConnectionsPostsVisibility = userSettings.AvailableSettings.ConnectionsPostsVisibility,
                    MyRepostsVisibility = userSettings.AvailableSettings.MyRepostsVisibility,
                    RepostsOfMyConnectionsVisibility = userSettings.AvailableSettings.RepostsOfMyConnectionsVisibility,
                    OrganizationIAmCreatorVisibility = userSettings.AvailableSettings.OrganizationIAmCreatorVisibility,
                    OrganizationIFollowVisibility = userSettings.AvailableSettings.OrganizationIFollowVisibility,

                    FrontendVersion = userSettings.AvailableSettings.FrontendVersion,
                    PreferredLanguage = userSettings.AvailableSettings.PreferredLanguage
                };
                await _repository.UpdateAsync(userSettingsDocument);
            }
        }
    }
}
