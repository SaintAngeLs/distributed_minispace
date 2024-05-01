using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Exceptions
{
    public class ReactionNotFoundException : AppException
    {
        public override string Code { get; } = "reaction_not_found";
        public Guid StudentId { get; }
        public Guid ContentId {get;}
        public ReactionContentType ContentType {get;}

        public ReactionNotFoundException(Guid studentId, Guid contentId,
                    ReactionContentType contentType) : 
                    base($"Reaction with student id: {studentId} and {Enum.GetName<ReactionContentType>(contentType)
                        .ToLower()} id: {contentId} was not found.")
        {
            StudentId = studentId;
            ContentId = contentId;
            ContentType = contentType;
        }
    }    
}
