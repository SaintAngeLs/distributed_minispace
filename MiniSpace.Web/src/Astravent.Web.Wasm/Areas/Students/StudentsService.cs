using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Notifications;
using Astravent.Web.Wasm.Areas.Students.CommandsDto;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Interests;
using Astravent.Web.Wasm.DTO.Languages;
using Astravent.Web.Wasm.DTO.Users;
using Astravent.Web.Wasm.DTO.Views;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Students
{
    public class StudentsService : IStudentsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        private readonly INotificationsService _notificationsService;

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

        public async Task<PaginatedResponseDto<StudentDto>> GetStudentsAsync()
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<PaginatedResponseDto<StudentDto>>("students");
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
            string phoneNumber,
            string country,
            string city,
            DateTime? dateOfBirth)
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
                languages = languages.ToList(),
                interests = interests.ToList(),
                enableTwoFactor,
                disableTwoFactor,
                twoFactorSecret,
                education,
                work,
                phoneNumber,
                country,
                city,
                dateOfBirth
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
                ProfileImageVisibility = availableSettings.ProfileImageVisibility.ToString(),  
                BannerImageVisibility = availableSettings.BannerImageVisibility.ToString(),    
                GalleryVisibility = availableSettings.GalleryVisibility.ToString(),          
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
                languages = languages.ToList(),
                interests = interests.ToList()
            };

            var jsonData = JsonSerializer.Serialize(updateData);
            Console.WriteLine($"Sending UpdateStudentLanguagesAndInterests request: {jsonData}");

            await _httpClient.PutAsync($"students/{studentId}/languages-and-interests", updateData);
        }

        public async Task<bool> IsUserOnlineAsync(Guid studentId)
        {
            return await _notificationsService.IsUserConnectedAsync(studentId);
        }

        public async Task ViewUserProfileAsync(Guid userId, Guid userProfileId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var command = new ViewUserProfileCommand(userId, userProfileId);
            await _httpClient.PostAsync("students/profiles/users/{userProfileId}/view", command);
        }

        public async Task<PaginatedResponseDto<UserProfileViewDto>> GetUserProfileViewsAsync(Guid userId, int pageNumber, int pageSize)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            
            var queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            return await _httpClient.GetAsync<PaginatedResponseDto<UserProfileViewDto>>($"students/profiles/users/{userId}/views/paginated{queryString}");
        }

         public async Task BlockUserAsync(Guid blockerId, Guid blockedUserId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var command = new { blockerId, blockedUserId };
            await _httpClient.PostAsync<object, object>($"students/{blockerId}/block-user/{blockedUserId}", command);
        }

        public async Task UnblockUserAsync(Guid blockerId, Guid blockedUserId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var command = new { blockerId, blockedUserId };
            await _httpClient.PostAsync<object, object>($"students/{blockerId}/unblock-user/{blockedUserId}", command);
        }

        public async Task<PagedResponseDto<BlockedUserDto>> GetBlockedUsersAsync(Guid blockerId, int page, int resultsPerPage)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var queryString = $"?page={page}&resultsPerPage={resultsPerPage}";
            return await _httpClient.GetAsync<PagedResponseDto<BlockedUserDto>>($"students/{blockerId}/blocked-users{queryString}");
        }
    }
}
