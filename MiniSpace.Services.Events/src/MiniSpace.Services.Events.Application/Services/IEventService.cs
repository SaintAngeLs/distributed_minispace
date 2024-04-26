using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Wrappers;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventService
    {
        Task<PagedResponse<IEnumerable<EventDto>>> BrowseEventsAsync(SearchEvents command);
    }
}