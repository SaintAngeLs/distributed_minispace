using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Core.Entities
{
    public class FileSourceInfo: AggregateRoot
    {
        public Guid SourceId { get; set; }
        public ContextType SourceType { get; set; }
        public ObjectId FileId { get; set; }
        public string FileName { get; set; }
        
        public FileSourceInfo(Guid id, Guid sourceId, ContextType sourceType, ObjectId fileId, string fileName)
        {
            Id = id;
            SourceId = sourceId;
            SourceType = sourceType;
            FileId = fileId;
            FileName = fileName;
        }
    }
}