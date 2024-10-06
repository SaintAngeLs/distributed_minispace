using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Queries;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Queries.Handlers
{
    public class GetMediaFileHandler : IQueryHandler<GetMediaFile, FileDto>
    {
        private readonly IMongoRepository<FileSourceInfoDocument, Guid> _fileSourceInfoRepository;
        private readonly IS3Service _s3Service;
        private const string FileContentType = "image/webp";

        public GetMediaFileHandler(IMongoRepository<FileSourceInfoDocument, Guid> fileSourceInfoRepository,
            IS3Service s3Service)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _s3Service = s3Service;
        }

        public async Task<FileDto> HandleAsync(GetMediaFile query, CancellationToken cancellationToken)
        {
            var fileSourceInfo = await _fileSourceInfoRepository.GetAsync(query.MediaFileId);
            if (fileSourceInfo is null)
            {
                return null;
            }

            var fileStream = new MemoryStream();
            await _s3Service.DownloadFileAsync(fileSourceInfo.FileUrl, fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] fileContent = fileStream.ToArray();
            var base64String = Convert.ToBase64String(fileContent);

            return new FileDto(query.MediaFileId, fileSourceInfo.SourceId, fileSourceInfo.SourceType.ToString(),
                fileSourceInfo.UploaderId, fileSourceInfo.State.ToString().ToLower(), fileSourceInfo.CreatedAt,
                fileSourceInfo.FileName, FileContentType, base64String);
        }
    }
}
