using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Dto.Events;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IEventsServiceClient
    {
        Task<EventParticipantsDto> GetParticipantsAsync(Guid eventId);
        Task<EventDto> GetEventAsync(Guid eventId);
    }
}