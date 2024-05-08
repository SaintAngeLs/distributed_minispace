using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using MiniSpace.Services.MediaFiles.Application.Queries;
using MiniSpace.Services.MediaFiles.Application.Services;
using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Queries.Handlers
{
    public class GetMediaFilesHandler : IQueryHandler<GetMediaFile, FileStreamResult>
    {
        private readonly IGridFSService _gridFSService;

        public GetMediaFilesHandler(IGridFSService gridFSService)
        {
            _gridFSService = gridFSService;
        }

        public async Task<FileStreamResult> HandleAsync(GetMediaFile query, CancellationToken cancellationToken)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(query.MediaFileId.ToString(), out objectId))
            {
                return null;
            }

            var fileStream = new MemoryStream();
            await _gridFSService.DownloadFileAsync(objectId, fileStream);

            fileStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(fileStream, "image/jpeg");
        }
    }
}