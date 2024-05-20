using MongoDB.Bson;

namespace MiniSpace.Services.MediaFiles.Core.Entities
{
    public class FileSourceInfo: AggregateRoot
    {
        public Guid SourceId { get; set; }
        public ContextType SourceType { get; set; }
        public Guid UploaderId { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
        public ObjectId OriginalFileId { get; set; }
        public string OriginalFileContentType { get; set; }
        public ObjectId FileId { get; set; }
        public string FileName { get; set; }
        
        public FileSourceInfo(Guid id, Guid sourceId, ContextType sourceType, Guid uploaderId, State state,
            DateTime createdAt, ObjectId originalFileId, string originalFileContentType, ObjectId fileId, string fileName)
        {
            Id = id;
            SourceId = sourceId;
            SourceType = sourceType;
            UploaderId = uploaderId;
            State = state;
            CreatedAt = createdAt;
            OriginalFileId = originalFileId;
            OriginalFileContentType = originalFileContentType;
            FileId = fileId;
            FileName = fileName;
        }
        
        public void Associate()
        {
            State = State.Associated;
        }
        
        public void Unassociate()
        {
            State = State.Unassociated;
        }
    }
}