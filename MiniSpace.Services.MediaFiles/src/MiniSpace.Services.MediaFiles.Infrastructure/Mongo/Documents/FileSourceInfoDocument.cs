using Convey.Types;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents
{
    public class FileSourceInfoDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public ContextType SourceType { get; set; }
        public Guid UploaderId { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
        public ObjectId FileId { get; set; }
        public string FileName { get; set; }
    }
}