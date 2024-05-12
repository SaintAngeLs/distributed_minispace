using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class StudentAlreadyGaveReactionException : AppException
    {
        public Guid StudentId { get; }
        public Guid ContentId { get; }
        public ReactionContentType ContentType { get; }

        public StudentAlreadyGaveReactionException(Guid studentId, Guid contentId, ReactionContentType contentType)
            : base($"Student with id: '{studentId}' already gave a reaction to the content with id: '{contentId}' and content type: '{contentType}'.")
        {
            StudentId = studentId;
            ContentId = contentId;
            ContentType = contentType;
        }
    }
}