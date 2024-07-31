namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IFileValidator
    {
        void ValidateFileSize(long size);
        void ValidateFileExtensions(byte[] bytes, string contentType);
    }
}
