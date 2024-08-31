using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class UserEventsViewsDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<ViewDto> Views { get; set; }
    }
}
