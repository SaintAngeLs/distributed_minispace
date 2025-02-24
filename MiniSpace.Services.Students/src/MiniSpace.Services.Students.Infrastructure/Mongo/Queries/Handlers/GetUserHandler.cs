using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUserGalleryRepository _userGalleryRepository;

        public GetUserHandler(
            IMongoRepository<UserDocument, Guid> userRepository,
            IUserSettingsRepository userSettingsRepository,
            IUserGalleryRepository userGalleryRepository)
        {
            _userRepository = userRepository;
            _userSettingsRepository = userSettingsRepository;
            _userGalleryRepository = userGalleryRepository;
        }

        public async Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken)
        {
            // Fetch the student document from the repository
            var studentDocument = await _userRepository.GetAsync(p => p.Id == query.UserId);
            if (studentDocument == null)
            {
                return null;
            }

            // Convert the student document to DTO
            var studentDto = studentDocument.AsDto();

            // Fetch the user settings from the repository
            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(query.UserId);
            if (userSettings != null)
            {
                // Map user settings to UserSettingsDto and include them in the UserDto
                studentDto.UserSettings = new UserSettingsDto
                {
                    UserId = userSettings.UserId,
                    CreatedAtVisibility = userSettings.AvailableSettings.CreatedAtVisibility.ToString(),
                    DateOfBirthVisibility = userSettings.AvailableSettings.DateOfBirthVisibility.ToString(),
                    InterestedInEventsVisibility = userSettings.AvailableSettings.InterestedInEventsVisibility.ToString(),
                    SignedUpEventsVisibility = userSettings.AvailableSettings.SignedUpEventsVisibility.ToString(),
                    EducationVisibility = userSettings.AvailableSettings.EducationVisibility.ToString(),
                    WorkPositionVisibility = userSettings.AvailableSettings.WorkPositionVisibility.ToString(),
                    LanguagesVisibility = userSettings.AvailableSettings.LanguagesVisibility.ToString(),
                    InterestsVisibility = userSettings.AvailableSettings.InterestsVisibility.ToString(),
                    ContactEmailVisibility = userSettings.AvailableSettings.ContactEmailVisibility.ToString(),
                    PhoneNumberVisibility = userSettings.AvailableSettings.PhoneNumberVisibility.ToString(),
                    ProfileImageVisibility = userSettings.AvailableSettings.ProfileImageVisibility.ToString(),
                    BannerImageVisibility = userSettings.AvailableSettings.BannerImageVisibility.ToString(),
                    GalleryVisibility = userSettings.AvailableSettings.GalleryVisibility.ToString(),
                    PreferredLanguage = userSettings.AvailableSettings.PreferredLanguage.ToString(),
                    FrontendVersion = userSettings.AvailableSettings.FrontendVersion.ToString()
                };
            }

            // Fetch the gallery images from the repository
            var userGallery = await _userGalleryRepository.GetAsync(query.UserId);
            if (userGallery != null)
            {
                studentDto.GalleryOfImageUrls = userGallery.GalleryOfImages
                    .Select(g => new GalleryImageDto(g.ImageId, g.ImageUrl, g.DateAdded))
                    .ToList();
            }

            return studentDto;
        }
    }
}
