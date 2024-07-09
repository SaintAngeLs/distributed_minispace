using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Repositories
{
    public class FileSourceInfoMongoRepository : IFileSourceInfoRepository
    {
        private readonly IMongoRepository<FileSourceInfoDocument, Guid> _repository;

        public FileSourceInfoMongoRepository(IMongoRepository<FileSourceInfoDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<FileSourceInfo> GetAsync(string url)
        {
            var fileSourceInfo = await _repository.GetAsync(s => s.OriginalFileUrl == url || s.FileUrl == url);
            return fileSourceInfo?.AsEntity();
        }
        
        public async Task<IEnumerable<FileSourceInfo>> GetAllUnassociatedAsync()
        {
            var fileSourceInfos = await _repository.FindAsync(s => s.State == State.Unassociated);
            return fileSourceInfos?.Select(s => s.AsEntity());
        }

        public Task AddAsync(FileSourceInfo fileSourceInfo)
            => _repository.AddAsync(fileSourceInfo.AsDocument());
        
        public Task UpdateAsync(FileSourceInfo fileSourceInfo)
            => _repository.UpdateAsync(fileSourceInfo.AsDocument());

        public async Task DeleteAsync(string url)
        {
            var fileSourceInfo = await _repository.GetAsync(s => s.OriginalFileUrl == url || s.FileUrl == url);
            if (fileSourceInfo != null)
            {
                await _repository.DeleteAsync(fileSourceInfo.Id);
            }
        }
        
        public Task<bool> ExistsAsync(string url)
            => _repository.ExistsAsync(s => s.OriginalFileUrl == url || s.FileUrl == url);
        
        public async Task<IEnumerable<FileSourceInfo>> FindAsync(Guid sourceId, ContextType sourceType)
        {
            var fileSourceInfos = await _repository.FindAsync(
                s => s.SourceId == sourceId && s.SourceType == sourceType);
            return fileSourceInfos?.Select(s => s.AsEntity());
        }

        public async Task<IEnumerable<FileSourceInfo>> FindByUploaderIdAndSourceTypeAsync(Guid uploaderId, ContextType sourceType)
        {
            var fileSourceInfos = await _repository.FindAsync(s => s.UploaderId == uploaderId && s.SourceType == sourceType);
            return fileSourceInfos?.Select(s => s.AsEntity());
        }
    }
}
