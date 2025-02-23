using System.Text.RegularExpressions;
using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetStudentsHandler : IQueryHandler<GetUsers, Application.Queries.PagedResult<UserDto>>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private const string BaseUrl = "students"; 

        public GetStudentsHandler(
            IMongoRepository<UserDocument, Guid> userRepository,
            IUserSettingsRepository userSettingsRepository)
        {
            _userRepository = userRepository;
            _userSettingsRepository = userSettingsRepository;
        }
        
        public async Task<Application.Queries.PagedResult<UserDto>> HandleAsync(GetUsers query, CancellationToken cancellationToken)
        {
            var filter = Builders<UserDocument>.Filter.Empty;
            
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                string searchTerm = query.Name.Trim();
                var parts = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var filters = new List<FilterDefinition<UserDocument>>();

                if (parts.Length == 1)
                {
                    var regex = new BsonRegularExpression(new Regex(parts[0], RegexOptions.IgnoreCase));
                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.FirstName, regex));
                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.LastName, regex));
                }
                else if (parts.Length >= 2)
                {
                    var firstNameRegex = new BsonRegularExpression(new Regex(parts[0], RegexOptions.IgnoreCase));
                    var lastNameRegex = new BsonRegularExpression(new Regex(parts[1], RegexOptions.IgnoreCase));

                    filters.Add(Builders<UserDocument>.Filter.And(
                        Builders<UserDocument>.Filter.Regex(x => x.FirstName, firstNameRegex),
                        Builders<UserDocument>.Filter.Regex(x => x.LastName, lastNameRegex)
                    ));
        
                    filters.Add(Builders<UserDocument>.Filter.And(
                        Builders<UserDocument>.Filter.Regex(x => x.FirstName, lastNameRegex),
                        Builders<UserDocument>.Filter.Regex(x => x.LastName, firstNameRegex)
                    ));

                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.FirstName, firstNameRegex));
                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.LastName, firstNameRegex));
                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.FirstName, lastNameRegex));
                    filters.Add(Builders<UserDocument>.Filter.Regex(x => x.LastName, lastNameRegex));
                }

                filter &= Builders<UserDocument>.Filter.Or(filters);
            }

            var options = new FindOptions<UserDocument, UserDocument>
            {
                Limit = query.ResultsPerPage,
                Skip = (query.Page - 1) * query.ResultsPerPage
            };

            using (var cursor = await _userRepository.Collection.FindAsync(filter, options, cancellationToken))
            {
                var documents = await cursor.ToListAsync(cancellationToken);
                var dtos = new List<UserDto>();

                foreach (var document in documents)
                {
                    var studentDto = document.AsDto();

                    // Fetch the user settings for each student and add to DTO
                    var userSettings = await _userSettingsRepository.GetUserSettingsAsync(document.Id);
                    if (userSettings != null)
                    {
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

                    dtos.Add(studentDto);
                }

                var total = await _userRepository.Collection.CountDocumentsAsync(filter);
                return new Application.Queries.PagedResult<UserDto>(dtos, (int)total, query.ResultsPerPage, query.Page, BaseUrl);
            }
        }
    }
}
