namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class InvalidOrganizationNameException : AppException
    {
        public override string Code { get; } = "invalid_organization_name";
        public string Name { get; }

        public InvalidOrganizationNameException(string name) : base($"Invalid organization name: {name}.")
        {
            Name = name;
        }
    }
}