using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Interests;
using MiniSpace.Web.DTO.Languages;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Students
{
    public class StudentsService : IStudentsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public StudentDto StudentDto { get; private set; }
        
        public StudentsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task UpdateStudentDto(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            StudentDto = await _httpClient.GetAsync<StudentDto>($"students/{studentId}");
        }

        public void ClearStudentDto()
        {
            StudentDto = null;
        }
        
        public async Task<StudentDto> GetStudentAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentDto>($"students/{studentId}");
        }

        public Task<PaginatedResponseDto<StudentDto>> GetStudentsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<PaginatedResponseDto<StudentDto>>("students");
        }

        public async Task UpdateStudentAsync(
            Guid studentId, 
            string firstName, 
            string lastName, 
            string profileImageUrl, 
            string description, 
            bool emailNotifications, 
            string contactEmail, 
            IEnumerable<string> languages, 
            IEnumerable<string> interests, 
            bool enableTwoFactor, 
            bool disableTwoFactor, 
            string twoFactorSecret,
            IEnumerable<EducationDto> education,
            IEnumerable<WorkDto> work,
            string phoneNumber)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updateStudentData = new
            {
                studentId,
                firstName,
                lastName,
                profileImageUrl,
                description,
                emailNotifications,
                contactEmail,
                languages = languages.Select(l => l.ToString()).ToList(),
                interests = interests.Select(i => i.ToString()).ToList(),
                enableTwoFactor,
                disableTwoFactor,
                twoFactorSecret,
                education,
                work,
                phoneNumber
            };

            var jsonData = JsonSerializer.Serialize(updateStudentData);
            Console.WriteLine($"Sending UpdateStudent request: {jsonData}");

            await _httpClient.PutAsync($"students/{studentId}", updateStudentData);
        }

        public async Task<NotificationPreferencesDto> GetUserNotificationPreferencesAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<NotificationPreferencesDto>($"students/{studentId}/notifications");
        }

        public Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImageUrl, string description, DateTime dateOfBirth, bool emailNotifications, string contactEmail)
            => _httpClient.PostAsync<object, object>("students", new { studentId, profileImageUrl, description, dateOfBirth, emailNotifications, contactEmail });

        public async Task<string> GetStudentStateAsync(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);
            return student != null ? student.State : "invalid";
        }

        // Updated method for notification preferences
        public async Task UpdateUserNotificationPreferencesAsync(Guid studentId, NotificationPreferencesDto preferencesDto, bool emailNotifications)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updatePreferencesData = new
            {
                studentId,
                emailNotifications,
                preferencesDto.AccountChanges,
                preferencesDto.SystemLogin,
                preferencesDto.NewEvent,
                preferencesDto.InterestBasedEvents,
                preferencesDto.EventNotifications,
                preferencesDto.CommentsNotifications,
                preferencesDto.PostsNotifications,
                preferencesDto.FriendsNotifications
            };

            // Serialize the data to JSON and log it
            var jsonData = JsonSerializer.Serialize(updatePreferencesData);
            Console.WriteLine($"Sending UpdateUserNotificationPreferences request: {jsonData}");

            await _httpClient.PostAsync($"students/{studentId}/notifications", updatePreferencesData);
        }

        public async Task<StudentWithGalleryImagesDto> GetStudentWithGalleryImagesAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentWithGalleryImagesDto>($"students/{studentId}/gallery");
        }

        public async Task UpdateUserSettingsAsync(Guid studentId, AvailableSettingsDto availableSettings)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updateUserSettingsData = new
            {
                studentId,
                CreatedAtVisibility = availableSettings.CreatedAtVisibility.ToString(),
                DateOfBirthVisibility = availableSettings.DateOfBirthVisibility.ToString(),
                InterestedInEventsVisibility = availableSettings.InterestedInEventsVisibility.ToString(),
                SignedUpEventsVisibility = availableSettings.SignedUpEventsVisibility.ToString(),
                EducationVisibility = availableSettings.EducationVisibility.ToString(),
                WorkPositionVisibility = availableSettings.WorkPositionVisibility.ToString(),
                LanguagesVisibility = availableSettings.LanguagesVisibility.ToString(),
                InterestsVisibility = availableSettings.InterestsVisibility.ToString(),
                ContactEmailVisibility = availableSettings.ContactEmailVisibility.ToString(),
                PhoneNumberVisibility = availableSettings.PhoneNumberVisibility.ToString(),
                PreferredLanguage = availableSettings.PreferredLanguage.ToString(),
                FrontendVersion = availableSettings.FrontendVersion.ToString()
            };

            await _httpClient.PutAsync($"students/{studentId}/settings", updateUserSettingsData);
        }

        public async Task<AvailableSettingsDto> GetUserSettingsAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<AvailableSettingsDto>($"students/{studentId}/settings");
        }

        public async Task UpdateStudentLanguagesAndInterestsAsync(
            Guid studentId, 
            IEnumerable<string> languages, 
            IEnumerable<string> interests)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updateData = new
            {
                languages = languages.Select(l => l.ToString()).ToList(),
                interests = interests.Select(i => i.ToString()).ToList()
            };

            var jsonData = JsonSerializer.Serialize(updateData);
            Console.WriteLine($"Sending UpdateStudentLanguagesAndInterests request: {jsonData}");

            await _httpClient.PutAsync($"students/{studentId}/languages-and-interests", updateData);
        }
    }
}
