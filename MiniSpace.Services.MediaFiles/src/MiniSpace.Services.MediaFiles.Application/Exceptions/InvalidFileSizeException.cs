namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class InvalidFileSizeException : AppException
    {
        public override string Code { get; } = "invalid_file_size";
        public int FileSize { get; }
        public int MaxFileSize { get; }

        public InvalidFileSizeException(int fileSize, int maxFileSize) 
            : base($"Invalid file size: {fileSize}. Maximum valid file size: {maxFileSize}.")
        {
            FileSize = fileSize;
            MaxFileSize = maxFileSize;
        }
    }
}