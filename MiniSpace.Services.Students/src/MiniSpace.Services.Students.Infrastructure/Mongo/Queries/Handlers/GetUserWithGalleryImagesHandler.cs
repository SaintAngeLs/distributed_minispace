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
    public class GetUserWithGalleryImagesHandler : IQueryHandler<GetUserWithGalleryImages, StudentWithGalleryImagesDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;
        private readonly IMongoRepository<UserGalleryDocument, Guid> _galleryRepository;

        public GetUserWithGalleryImagesHandler(IMongoRepository<UserDocument, Guid> userRepository, IMongoRepository<UserGalleryDocument, Guid> galleryRepository)
        {
            _userRepository = userRepository;
            _galleryRepository = galleryRepository;
        }
        
        public async Task<StudentWithGalleryImagesDto> HandleAsync(GetUserWithGalleryImages query, CancellationToken cancellationToken)
        {
            var studentDocument = await _userRepository.GetAsync(p => p.Id == query.UserId);
            if (studentDocument == null)
            {
                return null;
            }

            var galleryDocument = await _galleryRepository.GetAsync(g => g.UserId == query.UserId);
            var galleryImages = galleryDocument?.GalleryOfImages.Select(i => new GalleryImageDto(i.ImageId, i.ImageUrl, i.DateAdded)).ToList() ?? new List<GalleryImageDto>();

            return new StudentWithGalleryImagesDto
            {
                User = studentDocument.AsDto(),
                GalleryImages = galleryImages
            };
        }
    }
}
