﻿using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class MediaFileDeleted : IEvent
    {
        public string MediaFileUrl { get; }
        public Guid SourceId { get; }
        public string Source { get; }

        public MediaFileDeleted(string mediaFileUrl, Guid sourceId, string source)
        {
            MediaFileUrl = mediaFileUrl;
            SourceId = sourceId;
            Source = source;
        }
    }
}