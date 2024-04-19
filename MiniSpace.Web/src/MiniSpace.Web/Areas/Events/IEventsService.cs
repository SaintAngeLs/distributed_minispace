using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Events
{
    public interface IEventsService
    {
        Task<EventDto> GetEventAsync(Guid eventId);
        Task<PagedResponseDto<IEnumerable<EventDto>>> GetStudentEventsAsync(Guid studentId, int numberOfResults);
        Task AddEventAsync(Guid eventId, string name, Guid organizerId, DateTime startDate, DateTime endDate,
            string buildingName, string street, string buildingNumber, string apartmentNumber, string city,
            string zipCode, string description, int capacity, decimal fee, string category, DateTime publishDate);
        Task SignUpToEventAsync(Guid eventId, Guid studentId);
        Task ShowInterestInEventAsync(Guid eventId, Guid studentId);
        Task RateEventAsync(Guid eventId, int rating, Guid studentId);
        Task<PagedResponseDto<IEnumerable<EventDto>>> SearchEventsAsync(string name, string organizer, 
            DateTime dateFrom, DateTime dateTo, PageableDto pageable);
    }
}