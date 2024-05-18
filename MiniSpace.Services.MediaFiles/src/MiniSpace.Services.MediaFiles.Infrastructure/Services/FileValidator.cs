using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        private const int MaxFileSize = 200_000;

        public void ValidateFileSize(int size)
        {
            if (size > MaxFileSize)
            {
                throw new InvalidFileSizeException(size, MaxFileSize);
            }
        }
        
    }
}