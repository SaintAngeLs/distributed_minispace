using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class PostFileUploaded : IEvent
    {
        public Guid FileId { get; }
        public Guid PostId { get; }
        public string FileName { get; }
        public string FileUrl { get; }
        public string ContentType { get; }
        public DateTime UploadDate { get; }
        public Guid UploaderId { get; }

        public PostFileUploaded(Guid fileId, Guid postId, string fileName, string fileUrl, 
                                string contentType, DateTime uploadDate, Guid uploaderId)
        {
            FileId = fileId;
            PostId = postId;
            FileName = fileName;
            FileUrl = fileUrl;
            ContentType = contentType;
            UploadDate = uploadDate;
            UploaderId = uploaderId;
        }
    }
}
