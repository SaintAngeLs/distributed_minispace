namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidEventMediaFilesSizeException : AppException
    {
        public override string Code { get; } = "invalid_media_files_size";
        public int Size { get; }

        public InvalidEventMediaFilesSizeException(int size) : base($"Invalid media files size: {size}. It must be less or equal 5.")
        {
            Size = size;
        }
    }
}