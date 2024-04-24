namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class EmptyRequiredEventFieldException : AppException
    {
        public override string Code { get; } = "required_event_field_empty";
        public string FieldName { get; }

        public EmptyRequiredEventFieldException(string fieldName) : base($"Required event field: `{fieldName}` is empty.")
        {
            FieldName = fieldName;
        }
    }
}