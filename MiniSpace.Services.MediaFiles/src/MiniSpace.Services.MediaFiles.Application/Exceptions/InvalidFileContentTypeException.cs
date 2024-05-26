namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class InvalidFileContentTypeException : AppException
    {
        public override string Code { get; } = "invalid_file_content_type";
        public string ContentType { get; }

        public InvalidFileContentTypeException(string contentType) : base($"Invalid file content type: {contentType}.")
        {
            ContentType = contentType;
        }
    }
}