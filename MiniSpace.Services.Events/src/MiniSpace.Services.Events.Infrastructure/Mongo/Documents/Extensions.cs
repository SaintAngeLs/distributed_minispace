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
                MediaFilesUrl = document.MediaFiles,
                BannerUrl = document.BannerUrl, 
                InterestedStudents = document.InterestedStudents.Count(),
                SignedUpStudents = document.SignedUpStudents.Count(),
                Capacity = document.Capacity,
                Fee = document.Fee,
                Category = document.Category.ToString(),
                State = document.State.ToString(),
                PublishDate = document.PublishDate,
                UpdatedAt = document.UpdatedAt,
                Visibility = document.Visibility, 
                Settings = document.Settings.AsDto(),
                IsSignedUp = document.SignedUpStudents.Any(x => x.StudentId == studentId),
                IsInterested = document.InterestedStudents.Any(x => x.StudentId == studentId),
                StudentRating = document.Ratings.FirstOrDefault(x => x.StudentId == studentId)?.Value,
                FriendsInterestedIn = Enumerable.Empty<ParticipantDto>(),
                FriendsSignedUp = Enumerable.Empty<ParticipantDto>()
            };

        public static EventDto AsDto(this Event @event, Guid studentId)
        {
             return new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                Organizer = @event.Organizer.AsDto(),  // Assuming an AsDto method exists for Organizer
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Location = @event.Location.AsDto(),  // Assuming an AsDto method exists for Address
                MediaFilesUrl = @event.MediaFiles?.ToList(),  // Converting MediaFiles to a list of URLs
                BannerUrl = @event.BannerUrl,
                InterestedStudents = @event.InterestedParticipants.Count(),
                SignedUpStudents = @event.SignedUpParticipants.Count(),
                Capacity = @event.Capacity,
                Fee = @event.Fee,
                Category = @event.Category.ToString(),
                State = @event.State.ToString(),
                PublishDate = @event.PublishDate,
                UpdatedAt = @event.UpdatedAt,
                Visibility = @event.Visibility,
                Settings = new EventSettingsDto(@event.Settings), // Assuming an AsDto method exists for EventSettings
                IsSignedUp = @event.SignedUpParticipants.Any(x => x.StudentId == studentId),
                IsInterested = @event.InterestedParticipants.Any(x => x.StudentId == studentId),
                StudentRating = @event.Ratings.FirstOrDefault(x => x.StudentId == studentId)?.Value,
                FriendsInterestedIn = Enumerable.Empty<ParticipantDto>(),  // Placeholder, customize as needed
                FriendsSignedUp = Enumerable.Empty<ParticipantDto>()  // Placeholder, customize as needed
            };
        }

        public static OrganizerDto AsDto(this Organizer organizer)
        {
            return new OrganizerDto
            {
                Id = organizer.Id,
                UserId = organizer.UserId,
                OrganizationId = organizer.OrganizationId,
                OrganizerType = organizer.OrganizerType
            };
        }

        public static AddressDto AsDto(this Address address)
        {
            return new AddressDto
            {
                BuildingName = address.BuildingName,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
                ApartmentNumber = address.ApartmentNumber,
                City = address.City,
                ZipCode = address.ZipCode,
                Country = address.Country
            };
        }

         public static EventSettingsDto AsDto(this EventSettings settings)
        {
            return new EventSettingsDto(settings);
        }
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
            => new (document.Id, document.Name, document.Description, document.Organizer.ToEntity(),
                document.StartDate, document.EndDate, document.Location.ToEntity(), document.MediaFiles, 
                document.BannerUrl, document.Capacity, document.Fee, document.Category, 
                document.State, document.PublishDate, document.UpdatedAt, document.Visibility, 
                document.Settings.ToEntity(), document.InterestedStudents.Select(p => p.ToEntity()), 
                document.SignedUpStudents.Select(p => p.ToEntity()), document.Ratings.Select(r => r.ToEntity()));
        
        public static EventDocument AsDocument(this Event entity)
            => new ()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Organizer = OrganizerDocument.FromEntity(entity.Organizer),
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Location = AddressDocument.FromEntity(entity.Location),
                MediaFiles = entity.MediaFiles,
                BannerUrl = entity.BannerUrl,
                Capacity = entity.Capacity,
                Fee = entity.Fee,
                Category = entity.Category,
                State = entity.State,
                PublishDate = entity.PublishDate,
                UpdatedAt = entity.UpdatedAt,
                Visibility = entity.Visibility,
                Settings = EventSettingsDocument.FromEntity(entity.Settings),
                InterestedStudents = entity.InterestedParticipants.Select(p => ParticipantDocument.FromEntity(p)),
                SignedUpStudents = entity.SignedUpParticipants.Select(p => ParticipantDocument.FromEntity(p)),
                Ratings = entity.Ratings.Select(r => RatingDocument.FromEntity(r))
            };

        public static ParticipantDto AsDto(this ParticipantDocument document)
        {
            return new ParticipantDto
            {
                StudentId = document.StudentId
            };
        }

        public static ParticipantDocument AsDocument(this Participant entity)
        {
            return new ParticipantDocument
            {
                StudentId = entity.StudentId
            };
        }

        public static ParticipantDto ToDto(this ParticipantDocument document)
            => new ParticipantDto
            {
                StudentId = document.StudentId,
            };

            

        public static AddressDto AsDto(this AddressDocument document)
            => new ()
            {
                BuildingName = document.BuildingName,
                Street = document.Street,
                BuildingNumber = document.BuildingNumber,
                ApartmentNumber = document.ApartmentNumber,
                City = document.City,
                ZipCode = document.ZipCode,
                Country = document.Country
            };

        public static Address AsEntity(this AddressDocument document)
            => new (document.BuildingName, document.Street, document.BuildingNumber, 
                document.ApartmentNumber, document.City, document.ZipCode, document.Country);

        public static OrganizerDto AsDto(this OrganizerDocument document)
            => new ()
            {
                Id = document.Id,
                UserId = document.UserId,
                OrganizationId = document.OrganizationId,
                OrganizerType = document.OrganizerType
            };

        public static Organizer AsEntity(this OrganizerDocument document)
            => new (document.Id, document.OrganizerType, document.UserId, document.OrganizationId);

        public static EventSettingsDto AsDto(this EventSettingsDocument document)
        {
            var settings = document.ToEntity(); 
            return new EventSettingsDto(settings);
        }


        public static EventSettings AsEntity(this EventSettingsDocument document)
            => new ()
            {
                RequiresApproval = document.RequiresApproval,
                IsOnlineEvent = document.IsOnlineEvent,
                IsPrivate = document.IsPrivate,
                RequiresRSVP = document.RequiresRSVP,
                AllowsGuests = document.AllowsGuests,
                ShowAttendeesPublicly = document.ShowAttendeesPublicly,
                SendReminders = document.SendReminders,
                ReminderDaysBefore = document.ReminderDaysBefore,
                EnableChat = document.EnableChat,
                AllowComments = document.AllowComments,
                RequiresPayment = document.RequiresPayment,
                PaymentMethod = document.PaymentMethod,
                PaymentReceiverDetails = document.PaymentReceiverDetails,
                PaymentGateway = document.PaymentGateway,
                IssueTickets = document.IssueTickets,
                MaxTicketsPerPerson = document.MaxTicketsPerPerson,
                TicketPrice = document.TicketPrice,
                RecordEvent = document.RecordEvent,
                CustomTermsAndConditions = document.CustomTermsAndConditions,
                CustomFields = document.CustomFields
            };

        public static Participant AsEntity(this ParticipantDocument document)
            => new (document.StudentId);

        public static RatingDto AsRatingDto(this RatingDocument document)
            => new ()
            {
                StudentId = document.StudentId,
                Value = document.Value
            };

        public static Rating AsEntity(this RatingDocument document)
            => new (document.StudentId, document.Value);
    }
}
