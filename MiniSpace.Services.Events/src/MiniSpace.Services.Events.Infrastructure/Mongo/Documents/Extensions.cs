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
                IsSignedUp = document.SignedUpStudents.Any(x => x.UserId == studentId),
                IsInterested = document.InterestedStudents.Any(x => x.UserId == studentId),
                StudentRating = document.Ratings.FirstOrDefault(x => x.UserId == studentId)?.Value,
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
                Organizer = @event.Organizer.AsDto(),  
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Location = @event.Location.AsDto(),  
                MediaFilesUrl = @event.MediaFiles?.ToList(), 
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
                Settings = new EventSettingsDto(@event.Settings),
                IsSignedUp = @event.SignedUpParticipants.Any(x => x.UserId == studentId),
                IsInterested = @event.InterestedParticipants.Any(x => x.UserId == studentId),
                StudentRating = @event.Ratings.FirstOrDefault(x => x.UserId == studentId)?.Value,
                FriendsInterestedIn = Enumerable.Empty<ParticipantDto>(), 
                FriendsSignedUp = Enumerable.Empty<ParticipantDto>()  
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
                .Where(x => friends.Any(f => f.FriendId == x.UserId))
                .Select(p => p.AsDto());
            eventDto.FriendsSignedUp = document.SignedUpStudents
                .Where(x => friends.Any(f => f.FriendId == x.UserId))
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
                UserId = document.UserId
            };
        }

        public static ParticipantDocument AsDocument(this Participant entity)
        {
            return new ParticipantDocument
            {
                UserId = entity.UserId
            };
        }

        public static ParticipantDto ToDto(this ParticipantDocument document)
            => new ParticipantDto
            {
                UserId = document.UserId,
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
            => new (document.UserId);

        public static RatingDto AsRatingDto(this RatingDocument document)
            => new ()
            {
                StudentId = document.UserId,
                Value = document.Value
            };

        public static Rating AsEntity(this RatingDocument document)
            => new (document.UserId, document.Value);

        
        public static CommentDocument AsDocument(this Comment comment)
        {
            return new CommentDocument
            {
                Id = comment.Id,
                ContextId = comment.ContextId,
                CommentContext = comment.CommentContext,
                UserId = comment.UserId,
                ParentId = comment.ParentId,
                TextContent = comment.TextContent,
                CreatedAt = comment.CreatedAt,
                LastUpdatedAt = comment.LastUpdatedAt,
                RepliesCount = comment.RepliesCount,
                IsDeleted = comment.IsDeleted
            };
        }

        public static Comment AsEntity(this CommentDocument document)
        {
            return new Comment(
                document.Id,
                document.ContextId,
                document.CommentContext,
                document.UserId,
                document.ParentId,
                document.TextContent,
                document.CreatedAt,
                document.LastUpdatedAt,
                document.RepliesCount,
                document.IsDeleted
            );
        }

        public static UserCommentsDocument AsDocument(this IEnumerable<Comment> comments, Guid userId)
        {
            return new UserCommentsDocument
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Comments = comments.Select(comment => comment.AsDocument()).ToList()
            };
        }

        public static IEnumerable<Comment> AsEntities(this UserCommentsDocument document)
        {
            return document.Comments.Select(doc => doc.AsEntity());
        }

        public static ReactionDocument AsDocument(this Reaction reaction)
        {
            return new ReactionDocument
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                ContentId = reaction.ContentId,
                ContentType = reaction.ContentType,
                ReactionType = reaction.Type,
                TargetType = reaction.TargetType,
                CreatedAt = reaction.CreatedAt
            };
        }

         public static Reaction AsEntity(this ReactionDocument document)
        {
            return Reaction.Create(
                document.Id,
                document.UserId,
                document.ReactionType,
                document.ContentId,
                document.ContentType,
                document.TargetType
            );
        }

        public static UserReactionDocument AsDocument(this IEnumerable<Reaction> reactions, Guid userId)
        {
            return new UserReactionDocument
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Reactions = reactions.Select(reaction => reaction.AsDocument()).ToList()
            };
        }

        public static IEnumerable<Reaction> AsEntities(this UserReactionDocument document)
        {
            return document.Reactions.Select(doc => doc.AsEntity());
        }
    }
}
