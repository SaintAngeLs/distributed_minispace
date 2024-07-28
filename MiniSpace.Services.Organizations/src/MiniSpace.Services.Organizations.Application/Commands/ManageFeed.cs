using Convey.CQRS.Commands;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class ManageFeed : ICommand
    {
        public Guid OrganizationId { get; }
        public string Content { get; }
        public string Action { get; }

        public ManageFeed(Guid organizationId, string content, string action)
        {
            OrganizationId = organizationId;
            Content = content;
            Action = action;
        }
    }
}
