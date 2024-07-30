using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Interests;
using MiniSpace.Web.DTO.Languages;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Students
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
            IEnumerable<Language> languages, 
            IEnumerable<Interest> interests, 
            bool enableTwoFactor, 
            bool disableTwoFactor, 
            string twoFactorSecret,
            IEnumerable<EducationDto> education,
            IEnumerable<WorkDto> work,
            string phoneNumber);
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
            IEnumerable<Language> languages, 
            IEnumerable<Interest> interests);

    }
}
