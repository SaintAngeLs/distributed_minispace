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
    public class GetStudentHandler : IQueryHandler<GetStudent, StudentDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUserGalleryRepository _userGalleryRepository;

        public GetStudentHandler(
            IMongoRepository<StudentDocument, Guid> studentRepository,
            IUserSettingsRepository userSettingsRepository,
            IUserGalleryRepository userGalleryRepository)
        {
            _studentRepository = studentRepository;
            _userSettingsRepository = userSettingsRepository;
            _userGalleryRepository = userGalleryRepository;
        }

        public async Task<StudentDto> HandleAsync(GetStudent query, CancellationToken cancellationToken)
        {
            // Fetch the student document from the repository
            var studentDocument = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
            if (studentDocument == null)
            {
                return null;
            }

            // Convert the student document to DTO
            var studentDto = studentDocument.AsDto();

            // Fetch the user settings from the repository
            var userSettings = await _userSettingsRepository.GetUserSettingsAsync(query.StudentId);
            if (userSettings != null)
            {
                // Map user settings to UserSettingsDto and include them in the StudentDto
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
            var userGallery = await _userGalleryRepository.GetAsync(query.StudentId);
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
