namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class InvalidNumberOfPostMediaFilesException : AppException
    {
        public override string Code { get; } = "invalid_number_of_post_media_files";
        public Guid PostId { get; }
        public int MediaSizeNumber { get; }

        public InvalidNumberOfPostMediaFilesException(Guid postId, int mediaFilesNumber) 
            : base($"Invalid media files number: {mediaFilesNumber} for post with ID: '{postId}'. It should be less or equal 3.")
        {
            PostId = postId;
            MediaSizeNumber = mediaFilesNumber;
        }
    }
}