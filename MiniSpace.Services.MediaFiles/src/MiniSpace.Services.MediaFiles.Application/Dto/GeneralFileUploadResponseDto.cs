using System;

namespace MiniSpace.Services.MediaFiles.Application.Dto
{
    public class GeneralFileUploadResponseDto
    {
        public Guid Id { get; }
        public string FileUrl { get; }

        public GeneralFileUploadResponseDto(Guid id, string fileUrl)
        {
            Id = id;
            FileUrl = fileUrl;
        }
    }
}
