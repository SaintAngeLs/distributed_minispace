using Paralax.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using MiniSpace.Services.MediaFiles.Application.Dto;

namespace MiniSpace.Services.MediaFiles.Application.Queries
{
    public class GetMediaFile : IQuery<FileDto>
    {
        public Guid MediaFileId { get; set; }
    }
}