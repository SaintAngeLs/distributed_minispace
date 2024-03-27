using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id, document.Email, document.CreatedAt,
                document.Name, document.Surname, document.Friends, document.ProfileImage,
                document.Description, document.DateOfBirth, document.EmailNotifications,
                document.InterestedInEvents, document.SignedUpEvents);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument()
            {
                Id = entity.Id,
                Email = entity.Email,
                Name = entity.Name,
                Surname = entity.Surname,
                Friends = entity.Friends,
                ProfileImage = entity.ProfileImage,
                Description = entity.Description,
                DateOfBirth = entity.DateOfBirth,
                EmailNotifications = entity.EmailNotifications,
                CreatedAt = entity.CreatedAt,
                InterestedInEvents = entity.InterestedInEvents,
                SignedUpEvents = entity.SignedUpEvents
            };

        public static StudentDto AsDto(this StudentDocument document)
            => new StudentDto()
            {
                Id = document.Id,
                Email = document.Email,
                Name = document.Name,
                Surname = document.Surname,
                Friends = document.Friends,
                ProfileImage = document.ProfileImage,
                Description = document.Description,
                DateOfBirth = document.DateOfBirth,
                EmailNotifications = document.EmailNotifications,
                IsBanned = false,
                IsOrganizer = false,
                CreatedAt = document.CreatedAt,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents
            };
    }    
}
