using System;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<StudentEventsDto> GetAsync(Guid id);
        Task<bool> StudentExistsAsync(Guid id);
    }
}