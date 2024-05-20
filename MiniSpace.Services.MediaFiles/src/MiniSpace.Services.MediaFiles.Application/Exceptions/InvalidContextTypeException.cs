namespace MiniSpace.Services.MediaFiles.Application.Exceptions
{
    public class InvalidContextTypeException : AppException
    {
        public override string Code { get; } = "invalid_context_type";
        public string ContextType { get; }

        public InvalidContextTypeException(string contextType) : base($"Invalid context type: {contextType}.")
        {
            ContextType = contextType;
        }
    }
}