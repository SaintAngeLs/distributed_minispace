using System.Linq;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static EventDto AsDto(this EventDocument document)
            => new EventDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                StartDate = document.StartDate,
                EndDate = document.EndDate,
                Organizers = document.Organizers.Select(x => x.AsDto()),
                Location = document.Location.AsDto(),
                InterestedStudents = document.InterestedStudents.Count(),
                RegisteredStudents = document.RegisteredStudents.Count(),
                Capacity = document.Capacity,
                Fee = document.Fee,
                Category = document.Category.ToString(),
                Status = document.Status.ToString(),
                Reactions = document.Reactions.Count(),
                DominantReaction = document.Reactions.GroupBy(r => r.Type).OrderByDescending(g => g.Count())
                    .Select(g => g.Key.ToString()).FirstOrDefault()
            };

        public static AddressDto AsDto(this Address entity)
            => new AddressDto
            {
                BuildingName = entity.BuildingName,
                Street = entity.Street,
                BuildingNumber = entity.BuildingNumber,
                ApartmentNumber = entity.ApartmentNumber,
                City = entity.City,
                ZipCode = entity.ZipCode
            };
        
        public static OrganizerDto AsDto(this Organizer entity)
            => new OrganizerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Organization = entity.Organization
            };
    }
}