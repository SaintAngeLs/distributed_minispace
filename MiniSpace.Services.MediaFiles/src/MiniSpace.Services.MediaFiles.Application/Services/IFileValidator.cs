﻿namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IFileValidator
    {
        public void ValidateFileSize(int size);
        public void ValidateFileExtensions(byte[] bytes, string contentType);
    }
}