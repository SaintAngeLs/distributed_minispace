using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventService
    {
        Task<PagedResponse<EventDto>> BrowseEventsAsync(SearchEvents command);
        Task<PagedResponse<EventDto>> BrowseOrganizerEventsAsync(SearchOrganizerEvents command);
    }
}