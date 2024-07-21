namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class InvalidContextTypeException : AppException
    {
        public override string Code { get; } = "invalid_context_type";
        public string ContextType { get; }

        public InvalidContextTypeException(string contextType)
            : base($"Invalid context type: {contextType}.")
        {
            ContextType = contextType;
        }
    }
}
