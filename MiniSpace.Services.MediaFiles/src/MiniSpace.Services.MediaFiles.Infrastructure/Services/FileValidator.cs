using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        private const int MaxFileSize = 200_000;

        private readonly Dictionary<string, string> _mimeTypes = new Dictionary<string, string>()
        {
            { "FFD8FFE0", "image/jpeg" },
            { "FFD8FFE1", "image/jpeg" },
            { "FFD8FFE2", "image/jpeg" },
            { "89504E47", "image/png" }, 
            { "47494638", "image/gif" }, 
            { "49492A00", "image/tiff" },
            { "4D4D002A", "image/tiff" },
            { "424D", "image/bmp" }      
        };

        public void ValidateFileSize(int size)
        {
            if (size > MaxFileSize)
            {
                throw new InvalidFileSizeException(size, MaxFileSize);
            }
        }

        public void ValidateFileExtensions(byte[] bytes, string contextType)
        {
            if (!_mimeTypes.ContainsValue(contextType))
            {
                throw new InvalidFileContextTypeException(contextType);
            }
        }
        
    }
}