using System;

namespace MiniSpace.Services.MediaFiles.Core.Entities
{
    public class FileSourceInfo : AggregateRoot
    {
        public Guid SourceId { get; set; }
        public ContextType SourceType { get; set; }
        public Guid UploaderId { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OriginalFileUrl { get; set; }
        public string OriginalFileContentType { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }  
        public Guid? PostId { get; set; }   

        public FileSourceInfo(Guid id, Guid sourceId, ContextType sourceType, Guid uploaderId, State state,
            DateTime createdAt, string originalFileUrl, string originalFileContentType, string fileUrl, string fileName,
            Guid? organizationId = null, Guid? eventId = null, Guid? postId = null) 
        {
            Id = id;
            SourceId = sourceId;
            SourceType = sourceType;
            UploaderId = uploaderId;
            State = state;
            CreatedAt = createdAt;
            OriginalFileUrl = originalFileUrl;
            OriginalFileContentType = originalFileContentType;
            FileUrl = fileUrl;
            FileName = fileName;
            OrganizationId = organizationId;
            EventId = eventId; 
            PostId = postId;    
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
