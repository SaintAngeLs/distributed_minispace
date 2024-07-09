using Convey.CQRS.Commands;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class DeleteMediaFile: ICommand
    {
        public string MediaFileUrl { get; set; }
    }
}