using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Interests;
using Astravent.Web.Wasm.DTO.Languages;
using Astravent.Web.Wasm.DTO.Users;
using Astravent.Web.Wasm.DTO.Views;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Students
{
    public interface IStudentsService
    {
        StudentDto StudentDto { get; }
        Task UpdateStudentDto(Guid studentId);
        void ClearStudentDto();
        Task<StudentDto> GetStudentAsync(Guid studentId);
        Task<PaginatedResponseDto<StudentDto>> GetStudentsAsync();
        Task UpdateStudentAsync(
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
            DateTime? dateOfBirth);
        public Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImageUrl,
            string description, DateTime dateOfBirth, bool emailNotifications, string contactEmail);
        Task<string> GetStudentStateAsync(Guid studentId);

        Task<NotificationPreferencesDto> GetUserNotificationPreferencesAsync(Guid studentId);
        Task UpdateUserNotificationPreferencesAsync(Guid studentId, NotificationPreferencesDto preferencesDto, bool emailNotifications);
        Task<StudentWithGalleryImagesDto> GetStudentWithGalleryImagesAsync(Guid studentId);
        Task UpdateUserSettingsAsync(Guid studentId, AvailableSettingsDto availableSettings);
        Task<AvailableSettingsDto> GetUserSettingsAsync(Guid studentId);
        Task UpdateStudentLanguagesAndInterestsAsync(
            Guid studentId, 
            IEnumerable<string> languages, 
            IEnumerable<string> interests);

        Task<bool> IsUserOnlineAsync(Guid studentId);

        Task ViewUserProfileAsync(Guid userId, Guid userProfileId);

        Task<PaginatedResponseDto<UserProfileViewDto>> GetUserProfileViewsAsync(Guid userId, int pageNumber, int pageSize);
        Task BlockUserAsync(Guid blockerId, Guid blockedUserId);
        Task UnblockUserAsync(Guid blockerId, Guid blockedUserId);
        Task<PagedResponseDto<BlockedUserDto>> GetBlockedUsersAsync(Guid blockerId, int page, int resultsPerPage);
    }
}
