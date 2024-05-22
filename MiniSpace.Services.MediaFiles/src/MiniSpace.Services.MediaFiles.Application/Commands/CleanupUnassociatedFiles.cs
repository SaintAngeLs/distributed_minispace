using Convey.CQRS.Commands;

namespace MiniSpace.Services.MediaFiles.Application.Commands
{
    public class CleanupUnassociatedFiles: ICommand
    {
        public DateTime Now { get; set; }
        
        public CleanupUnassociatedFiles(DateTime now)
        {
            Now = now;
        }
    }
}