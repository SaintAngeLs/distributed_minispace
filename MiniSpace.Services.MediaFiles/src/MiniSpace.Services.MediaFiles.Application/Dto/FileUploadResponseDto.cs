namespace MiniSpace.Services.MediaFiles.Application.Dto
{
    public class FileUploadResponseDto
    {
        public Guid FileId { get; set; }

        public FileUploadResponseDto(Guid fileId)
        {
            FileId = fileId;
        }
    }
}