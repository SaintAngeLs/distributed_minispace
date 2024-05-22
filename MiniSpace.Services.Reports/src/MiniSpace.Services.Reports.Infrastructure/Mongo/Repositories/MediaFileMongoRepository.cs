using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public class MediaFileMongoRepository : IMediaFileRepository
    {
        private readonly IMongoRepository<MediaFileDocument, Guid> _repository;

        public MediaFileMongoRepository(IMongoRepository<MediaFileDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<MediaFile> GetAsync(Guid id)
        {
            var mediaFile = await _repository.GetAsync(mf => mf.Id == id);

            return mediaFile?.AsEntity();
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(mf => mf.Id == id);

        public Task AddAsync(MediaFile mediaFile)
            => _repository.AddAsync(mediaFile.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}