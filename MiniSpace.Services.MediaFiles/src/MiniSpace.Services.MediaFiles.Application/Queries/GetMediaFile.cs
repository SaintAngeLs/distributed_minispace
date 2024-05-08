using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MiniSpace.Services.MediaFiles.Application.Queries
{
    public class GetMediaFile : IQuery<FileStreamResult>
    {
        public Guid MediaFileId { get; set; }
    }
}