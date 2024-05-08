using MiniSpace.Services.MediaFiles.Application.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class GridFSService: IGridFSService
    {
        private readonly IMongoDatabase _database;
        private readonly GridFSBucket _gridFSBucket;

        public GridFSService(IMongoDatabase database)
        {
            _database = database;
            _gridFSBucket = new GridFSBucket(_database);
        }

        public async Task<ObjectId> UploadFileAsync(string fileName, Stream fileStream)
        {
            ObjectId fileId = await _gridFSBucket.UploadFromStreamAsync(fileName, fileStream);
            return fileId;
        }

        public async Task DownloadFileAsync(ObjectId fileId, Stream destination)
        {
            await _gridFSBucket.DownloadToStreamAsync(fileId, destination);
        }
    }
}