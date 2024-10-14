using Paralax.Types;
using MiniSpace.Services.MediaFiles.Core.Entities;
using MongoDB.Bson;
using System;

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
        public string OriginalFileUrl { get; set; }
        public string OriginalFileContentType { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public Guid? OrganizationId { get; set; } 
        public Guid? EventId { get; set; }  
        public Guid? PostId { get; set; } 
    }
}
