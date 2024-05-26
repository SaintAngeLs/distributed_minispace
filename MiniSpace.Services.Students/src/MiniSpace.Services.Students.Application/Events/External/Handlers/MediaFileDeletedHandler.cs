using Convey.CQRS.Events;
using MiniSpace.Services.Students.Core.Repositories;

namespace MiniSpace.Services.Students.Application.Events.External.Handlers
{
    public class MediaFileDeletedHandler: IEventHandler<MediaFileDeleted>
    {
        private readonly IStudentRepository _studentRepository;
        
        public MediaFileDeletedHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task HandleAsync(MediaFileDeleted @event, CancellationToken cancellationToken)
        {
            if(@event.Source.ToLowerInvariant() != "studentprofile")
            {
                return;
            }

            var student = await _studentRepository.GetAsync(@event.SourceId);
            if(student != null)
            {
                student.RemoveProfileImage(@event.MediaFileId);
                await _studentRepository.UpdateAsync(student);
            }
        }
    }
}
