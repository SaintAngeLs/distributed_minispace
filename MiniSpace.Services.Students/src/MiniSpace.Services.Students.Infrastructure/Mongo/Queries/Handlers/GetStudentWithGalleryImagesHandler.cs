using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentWithGalleryImagesHandler : IQueryHandler<GetStudentWithGalleryImages, StudentWithGalleryImagesDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;
        private readonly IMongoRepository<UserGalleryDocument, Guid> _galleryRepository;

        public GetStudentWithGalleryImagesHandler(IMongoRepository<StudentDocument, Guid> studentRepository, IMongoRepository<UserGalleryDocument, Guid> galleryRepository)
        {
            _studentRepository = studentRepository;
            _galleryRepository = galleryRepository;
        }
        
        public async Task<StudentWithGalleryImagesDto> HandleAsync(GetStudentWithGalleryImages query, CancellationToken cancellationToken)
        {
            var studentDocument = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
            if (studentDocument == null)
            {
                return null;
            }

            var galleryDocument = await _galleryRepository.GetAsync(g => g.UserId == query.StudentId);
            var galleryImages = galleryDocument?.GalleryOfImages.Select(i => new GalleryImageDto(i.ImageId, i.ImageUrl, i.DateAdded)).ToList() ?? new List<GalleryImageDto>();

            return new StudentWithGalleryImagesDto
            {
                Student = studentDocument.AsDto(),
                GalleryImages = galleryImages
            };
        }
    }
}
