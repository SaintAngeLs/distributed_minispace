﻿using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Queries;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;
using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOriginalMediaFileHandler : IQueryHandler<GetOriginalMediaFile, FileDto>
    {
        private readonly IMongoRepository<FileSourceInfoDocument, Guid> _fileSourceInfoRepository;
        private readonly IGridFSService _gridFSService;

        public GetOriginalMediaFileHandler(IMongoRepository<FileSourceInfoDocument, Guid> fileSourceInfoRepository,
            IGridFSService gridFSService)
        {
            _fileSourceInfoRepository = fileSourceInfoRepository;
            _gridFSService = gridFSService;
        }

        public async Task<FileDto> HandleAsync(GetOriginalMediaFile query, CancellationToken cancellationToken)
        {
            var fileSourceInfo = await _fileSourceInfoRepository.GetAsync(query.MediaFileId);
            if (fileSourceInfo is null)
            {
                return null;
            }

            var fileStream = new MemoryStream();
            await _gridFSService.DownloadFileAsync(fileSourceInfo.OriginalFileId, fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] fileContent = fileStream.ToArray();
            var base64String = Convert.ToBase64String(fileContent);

            return new FileDto(query.MediaFileId, fileSourceInfo.SourceId, fileSourceInfo.SourceType.ToString(),
                fileSourceInfo.UploaderId, fileSourceInfo.State.ToString().ToLower(), fileSourceInfo.CreatedAt,
                fileSourceInfo.FileName, fileSourceInfo.OriginalFileContentType, base64String);
        }
    }
}