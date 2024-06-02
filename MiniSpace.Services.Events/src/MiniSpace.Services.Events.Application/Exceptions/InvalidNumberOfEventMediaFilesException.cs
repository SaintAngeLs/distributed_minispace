using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidNumberOfEventMediaFilesException : AppException
    {
        public override string Code { get; } = "invalid_number_of_event_media_files";
        public int MediaFilesNumber { get; }
        public InvalidNumberOfEventMediaFilesException(int mediaFilesNumber) 
            : base($"Invalid media files number: {mediaFilesNumber}. It must be less or equal 5.")
        {
            MediaFilesNumber = mediaFilesNumber;
        }
    }
}