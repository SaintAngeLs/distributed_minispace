using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        private const int MaxFileSize = 5_000_000;

        private readonly Dictionary<string, string> _mimeTypes = new Dictionary<string, string>()
        {
            { "FFD8FFDB", "image/jpeg" },
            { "FFD8FFE0", "image/jpeg" },
            { "FFD8FFE1", "image/jpeg" },
            { "FFD8FFE2", "image/jpeg" },
            { "FFD8FFEE", "image/jpeg" },
            { "89504E47", "image/png" }, 
            { "47494638", "image/gif" }, 
            { "49492A00", "image/tiff" },
            { "4D4D002A", "image/tiff" },
            { "52494646", "image/webp" },
            { "57454250", "image/webp" }
        };

        public void ValidateFileSize(int size)
        {
            if (size > MaxFileSize)
            {
                throw new InvalidFileSizeException(size, MaxFileSize);
            }
        }

        public void ValidateFileExtensions(byte[] bytes, string contentType)
        {
            if (!_mimeTypes.ContainsValue(contentType))
            {
                throw new InvalidFileContentTypeException(contentType);
            }
            
            string hex = BitConverter.ToString(bytes, 0, 4).Replace("-", string.Empty);
            _mimeTypes.TryGetValue(hex, out var mimeType);
            if (mimeType != contentType)
            {
                throw new FileTypeDoesNotMatchContentTypeException(mimeType, contentType);
            }
        }
        
    }
}