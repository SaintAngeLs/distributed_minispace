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

        public async Task<FileSourceInfo> GetAsync(Guid id)
        {
            var fileSourceInfo = await _repository.GetAsync(s => s.Id == id);

            return fileSourceInfo?.AsEntity();
        }

        public Task AddAsync(FileSourceInfo fileSourceInfo)
            => _repository.AddAsync(fileSourceInfo.AsDocument());
        
        public Task UpdateAsync(FileSourceInfo fileSourceInfo)
            => _repository.UpdateAsync(fileSourceInfo.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
        
        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(s => s.Id == id);
        
        public async Task<IEnumerable<FileSourceInfo>> FindAsync(Guid sourceId, ContextType sourceType)
        {
            var fileSourceInfos = await _repository.FindAsync(
                s => s.SourceId == sourceId && s.SourceType == sourceType);

            return fileSourceInfos?.Select(s => s.AsEntity());
        }
    }
}