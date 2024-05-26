using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IEventsServiceClient
    {
        Task<IEnumerable<StudentDto>> GetParticipantsAsync(Guid eventId);
    }
}