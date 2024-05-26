namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class FileTypeDoesNotMatchContentTypeException : AppException
    {
        public override string Code { get; } = "file_type_does_not_match_content_type";
        public string FileType { get; }
        public string ContentType { get; }

        public FileTypeDoesNotMatchContentTypeException(string fileType, string contentType)
            : base($"File extension: {fileType} is not matching content type: {contentType}")
        {
            FileType = fileType;
            ContentType = contentType;
        }
    }
}