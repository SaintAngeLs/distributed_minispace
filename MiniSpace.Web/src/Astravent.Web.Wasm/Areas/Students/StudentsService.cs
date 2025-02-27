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

        public UserDto UserDto { get; private set; }
        
        public StudentsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task UpdateStudentDto(Guid userId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            UserDto = await _httpClient.GetAsync<UserDto>($"students/{userId}");
        }

        public void ClearStudentDto()
        {
            UserDto = null;
        }
        
        public async Task<UserDto> GetStudentAsync(Guid userId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<UserDto>($"students/{userId}");
        }

        public async Task<PaginatedResponseDto<UserDto>> GetStudentsAsync()
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<PaginatedResponseDto<UserDto>>("students");
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

        public async Task<NotificationPreferencesDto> GetUserNotificationPreferencesAsync(Guid userId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<NotificationPreferencesDto>($"students/{userId}/notifications");
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
                preferencesDto.SystemLogin,
                preferencesDto.InterestBasedEvents,
                preferencesDto.EventNotifications,
                preferencesDto.CommentsNotifications,
                preferencesDto.PostsNotifications,
                preferencesDto.EventRecommendation,
                preferencesDto.FriendsRecommendation,
                preferencesDto.FriendsPosts,
                preferencesDto.PostsRecommendation,
                preferencesDto.EventsIAmInterestedInNotification,
                preferencesDto.EventsIAmSignedUpToNotification,
                preferencesDto.PostsOfPeopleIFollowNotification,
                preferencesDto.EventNotificationForPeopleIFollow,
                preferencesDto.NewFriendsRequests,
                preferencesDto.MyRequestsAccepted,
                preferencesDto.FriendsPostsNotifications
            };

            await _httpClient.PostAsync($"students/{studentId}/notifications", updatePreferencesData);
        }



        public async Task<StudentWithGalleryImagesDto> GetStudentWithGalleryImagesAsync(Guid userId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentWithGalleryImagesDto>($"students/{userId}/gallery");
        }

        public async Task UpdateUserSettingsAsync(Guid userId, AvailableSettingsDto availableSettings)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var updateUserSettingsData = new
            {
                UserId = userId,
                CreatedAtVisibility = GetSettingValue(availableSettings.CreatedAtVisibility, "Everyone"),
                DateOfBirthVisibility = GetSettingValue(availableSettings.DateOfBirthVisibility, "Everyone"),
                InterestedInEventsVisibility = GetSettingValue(availableSettings.InterestedInEventsVisibility, "Everyone"),
                SignedUpEventsVisibility = GetSettingValue(availableSettings.SignedUpEventsVisibility, "Everyone"),
                EducationVisibility = GetSettingValue(availableSettings.EducationVisibility, "Everyone"),
                WorkPositionVisibility = GetSettingValue(availableSettings.WorkPositionVisibility, "Everyone"),
                LanguagesVisibility = GetSettingValue(availableSettings.LanguagesVisibility, "Everyone"),
                InterestsVisibility = GetSettingValue(availableSettings.InterestsVisibility, "Everyone"),
                ContactEmailVisibility = GetSettingValue(availableSettings.ContactEmailVisibility, "Everyone"),
                PhoneNumberVisibility = GetSettingValue(availableSettings.PhoneNumberVisibility, "Everyone"),
                ProfileImageVisibility = GetSettingValue(availableSettings.ProfileImageVisibility, "Everyone"),
                BannerImageVisibility = GetSettingValue(availableSettings.BannerImageVisibility, "Everyone"),
                GalleryVisibility = GetSettingValue(availableSettings.GalleryVisibility, "Everyone"),
                ConnectionVisibility = GetSettingValue(availableSettings.ConnectionVisibility, "Everyone"),
                FollowersVisibility = GetSettingValue(availableSettings.FollowersVisibility, "Everyone"),
                FollowingVisibility = GetSettingValue(availableSettings.FollowingVisibility, "Everyone"),
                MyPostsVisibility = GetSettingValue(availableSettings.MyPostsVisibility, "Everyone"),
                ConnectionsPostsVisibility = GetSettingValue(availableSettings.ConnectionsPostsVisibility, "Everyone"),
                MyRepostsVisibility = GetSettingValue(availableSettings.MyRepostsVisibility, "Everyone"),
                RepostsOfMyConnectionsVisibility = GetSettingValue(availableSettings.RepostsOfMyConnectionsVisibility, "Everyone"),
                OrganizationIAmCreatorVisibility = GetSettingValue(availableSettings.OrganizationIAmCreatorVisibility, "Everyone"),
                OrganizationIFollowVisibility = GetSettingValue(availableSettings.OrganizationIFollowVisibility, "Everyone"),
                IsOnlineVisibility = GetSettingValue(availableSettings.IsOnlineVisibility, "Everyone"),
                DeviceTypeVisibility = GetSettingValue(availableSettings.DeviceTypeVisibility, "Everyone"),
                LastActiveVisibility = GetSettingValue(availableSettings.LastActiveVisibility, "Everyone"),
                CountryVisibility = GetSettingValue(availableSettings.CountryVisibility, "Everyone"),
                CityVisibility = GetSettingValue(availableSettings.CityVisibility, "Everyone"),
                PreferredLanguage = GetSettingValue(availableSettings.PreferredLanguage, "English"),
                FrontendVersion = GetSettingValue(availableSettings.FrontendVersion, "Default"),
                FriendListVisibility = GetSettingValue(availableSettings.FriendListVisibility, "Everyone"),
                FollowersListVisibility = GetSettingValue(availableSettings.FollowersListVisibility, "Everyone"),
                FollowingListVisibility = GetSettingValue(availableSettings.FollowingListVisibility, "Everyone"),
                MessageVisibility = GetSettingValue(availableSettings.MessageVisibility, "Everyone"),
                ProfileVisibility = GetSettingValue(availableSettings.ProfileVisibility, "Everyone"),
                PostCommentVisibility = GetSettingValue(availableSettings.PostCommentVisibility, "Everyone"),
                PostLikeVisibility = GetSettingValue(availableSettings.PostLikeVisibility, "Everyone"),
                FriendRequestVisibility = GetSettingValue(availableSettings.FriendRequestVisibility, "Everyone"),
                TaggedPostVisibility = GetSettingValue(availableSettings.TaggedPostVisibility, "Everyone"),
                StoryVisibility = GetSettingValue(availableSettings.StoryVisibility, "Everyone"),
                GroupMembershipVisibility = GetSettingValue(availableSettings.GroupMembershipVisibility, "Everyone")
            };

            await _httpClient.PutAsync($"students/{userId}/settings", updateUserSettingsData);
        }
        private string GetSettingValue(object setting, string defaultValue)
        {
            return setting?.ToString() ?? defaultValue;
        }


        public async Task<AvailableSettingsDto> GetUserSettingsAsync(Guid userId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<AvailableSettingsDto>($"students/{userId}/settings");
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
