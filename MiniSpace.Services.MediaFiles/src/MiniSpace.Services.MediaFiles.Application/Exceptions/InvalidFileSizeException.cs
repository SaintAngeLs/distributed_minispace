namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class InvalidFileSizeException : AppException
    {
        public override string Code { get; } = "invalid_file_size";
        public long FileSize { get; }
        public long MaxFileSize { get; }

        public InvalidFileSizeException(long fileSize, long maxFileSize) 
            : base($"Invalid file size: {fileSize}. Maximum valid file size: {maxFileSize}.")
        {
            FileSize = fileSize;
            MaxFileSize = maxFileSize;
        }
    }
}