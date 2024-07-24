using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
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
            IEnumerable<InterestDto> interests, 
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
                languages,
                interests,
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

        public Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImageUrl, string description, DateTime dateOfBirth, bool emailNotifications, string contactEmail)
            => _httpClient.PostAsync<object, object>("students", new { studentId, profileImageUrl, description, dateOfBirth, emailNotifications, contactEmail });

        public async Task<string> GetStudentStateAsync(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);
            return student != null ? student.State : "invalid";
        }

        // New methods for notification preferences
        public async Task<NotificationPreferencesDto> GetUserNotificationPreferencesAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<NotificationPreferencesDto>($"students/{studentId}/notifications");
        }

        public async Task UpdateUserNotificationPreferencesAsync(Guid studentId, NotificationPreferencesDto preferencesDto)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updatePreferencesData = new
            {
                studentId,
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
                availableSettings.CreatedAtVisibility,
                availableSettings.DateOfBirthVisibility,
                availableSettings.InterestedInEventsVisibility,
                availableSettings.SignedUpEventsVisibility,
                availableSettings.EducationVisibility,
                availableSettings.WorkPositionVisibility,
                availableSettings.LanguagesVisibility,
                availableSettings.InterestsVisibility,
                availableSettings.ContactEmailVisibility,
                availableSettings.PhoneNumberVisibility,
                availableSettings.PreferredLanguage,
                availableSettings.FrontendVersion
            };

            var jsonData = JsonSerializer.Serialize(updateUserSettingsData);
            Console.WriteLine($"Sending UpdateUserSettings request: {jsonData}");

            await _httpClient.PutAsync($"students/{studentId}/settings", updateUserSettingsData);
        }
    }
}
