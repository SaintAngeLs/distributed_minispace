using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id, document.Username, document.Password, document.Email,
                document.CreatedAt, document.Friends, document.ProfileImage, document.Description,
                document.DateOfBirth, document.EmailNotifications, document.IsBanned, document.IsOrganizer,
                document.InterestedInEvents, document.SignedUpEvents);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument()
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                Email = entity.Email,
                Friends = entity.Friends,
                ProfileImage = entity.ProfileImage,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                EmailNotifications = entity.EmailNotifications,
                IsBanned = entity.IsBanned,
                IsOrganizer = entity.IsOrganizer,
                CreatedAt = entity.CreatedAt,
                InterestedInEvents = entity.InterestedInEvents,
                SignedUpEvents = entity.SignedUpEvents
            };

        public static StudentDto AsDto(this StudentDocument document)
            => new StudentDto()
            {
                Id = document.Id,
                Username = document.Username,
                Password = document.Password,
                Email = document.Email,
                Friends = document.Friends,
                ProfileImage = document.ProfileImage,
                Description = document.Description,
                DateOfBirth = document.DateOfBirth,
                EmailNotifications = document.EmailNotifications,
                IsBanned = document.IsBanned,
                IsOrganizer = document.IsOrganizer,
                CreatedAt = document.CreatedAt,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents
            };
    }    
}
