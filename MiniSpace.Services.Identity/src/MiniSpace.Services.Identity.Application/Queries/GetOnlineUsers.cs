using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using MiniSpace.Services.Identity.Application.DTO;

namespace MiniSpace.Services.Identity.Application.Queries
{
    /// <summary>
    /// Very unsafe query!
    /// </summary> 
    /// <summary>
    /// Handler the statuses update withing the corresponding evnet 
    /// subscriber and handler in the User service.
    /// </summary>
    public class GetOnlineUsers : IQuery<PagedResult<UserDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
