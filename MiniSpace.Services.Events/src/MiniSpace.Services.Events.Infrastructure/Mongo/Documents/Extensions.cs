using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static EventDto AsDto(this EventDocument document, Guid studentId)
            => new ()
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                Organizer = document.Organizer.AsDto(),
                StartDate = document.StartDate,
                EndDate = document.EndDate,
                Location = document.Location.AsDto(),
                MediaFiles = document.MediaFiles,
                InterestedStudents = document.InterestedStudents.Count(),
                SignedUpStudents = document.SignedUpStudents.Count(),
                Capacity = document.Capacity,
                Fee = document.Fee,
                Category = document.Category.ToString(),
                Status = document.State.ToString(),
                PublishDate = document.PublishDate,
                UpdatedAt = document.UpdatedAt,
                IsSignedUp = document.SignedUpStudents.Any(x => x.StudentId == studentId),
                IsInterested = document.InterestedStudents.Any(x => x.StudentId == studentId),
                StudentRating = document.Ratings.FirstOrDefault(x => x.StudentId == studentId)?.Value,
            };

        public static EventDto AsDtoWithFriends(this EventDocument document, Guid studentId, IEnumerable<FriendDto> friends)
        {
            var eventDto = document.AsDto(studentId);
            eventDto.FriendsInterestedIn = document.InterestedStudents
                .Where(x => friends.Any(f => f.FriendId == x.StudentId))
                .Select(p => p.AsDto());
            eventDto.FriendsSignedUp = document.SignedUpStudents
                .Where(x => friends.Any(f => f.FriendId == x.StudentId))
                .Select(p => p.AsDto());
            return eventDto;
        }
        
        public static Event AsEntity(this EventDocument document)
            => new (document.Id, document.Name, document.Description, document.StartDate, document.EndDate,
                document.Location, document.MediaFiles, document.Capacity, document.Fee, document.Category, 
                document.State, document.PublishDate, document.Organizer, document.UpdatedAt,document.InterestedStudents, 
                document.SignedUpStudents, document.Ratings);
        
        public static EventDocument AsDocument(this Event entity)
            => new ()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Organizer = entity.Organizer,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Location = entity.Location,
                MediaFiles = entity.MediaFiles,
                InterestedStudents = entity.InterestedStudents,
                SignedUpStudents = entity.SignedUpStudents,
                Capacity = entity.Capacity,
                Fee = entity.Fee,
                Category = entity.Category,
                State = entity.State,
                PublishDate = entity.PublishDate,
                UpdatedAt = entity.UpdatedAt,
                Ratings = entity.Ratings
            };
        
        public static EventParticipantsDto AsDto(this EventDocument document)
            => new ()
            {
                EventId = document.Id,
                InterestedStudents = document.InterestedStudents.Select(p => p.AsDto()),
                SignedUpStudents = document.SignedUpStudents.Select(p => p.AsDto())
            };
        
        public static EventRatingDto AsRatingDto(this EventDocument document)
            => new ()
            {
                EventId = document.Id,
                TotalRatings = document.Ratings.Count(),
                AverageRating = document.Ratings.Any() ? document.Ratings.Average(x => x.Value) : 0
            };

        public static AddressDto AsDto(this Address entity)
            => new ()
            {
                BuildingName = entity.BuildingName,
                Street = entity.Street,
                BuildingNumber = entity.BuildingNumber,
                ApartmentNumber = entity.ApartmentNumber,
                City = entity.City,
                ZipCode = entity.ZipCode
            };
        
        public static Address AsEntity(this AddressDto dto)
            => new (dto.BuildingName, dto.Street, dto.BuildingNumber, dto.ApartmentNumber, dto.City, dto.ZipCode);
        
        public static OrganizerDto AsDto(this Organizer entity)
            => new ()
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                OrganizationId = entity.OrganizationId,
                OrganizationName = entity.OrganizationName
            };
        
        public static StudentDocument AsDocument(this Student entity)
            => new ()
            {
                Id = entity.Id,
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new (document.Id);
        
        public static ParticipantDto AsDto(this Participant entity)
            => new ()
            {
                StudentId = entity.StudentId,
                Name = entity.Name
            };
    }
}