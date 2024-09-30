using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserGalleryRepository : IUserGalleryRepository
    {
        private readonly IMongoRepository<UserGalleryDocument, Guid> _repository;

        public UserGalleryRepository(IMongoRepository<UserGalleryDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserGallery> GetAsync(Guid userId)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userId);
            return userGalleryDocument?.AsEntity();
        }

        public async Task AddAsync(UserGallery userGallery)
        {
            var userGalleryDocument = userGallery.AsDocument();
            await _repository.AddAsync(userGalleryDocument);
        }

        public async Task UpdateAsync(UserGallery userGallery)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userGallery.UserId);

            if (userGalleryDocument == null)
            {
                userGalleryDocument = userGallery.AsDocument();
                await _repository.AddAsync(userGalleryDocument);
            }
            else
            {
                userGalleryDocument.GalleryOfImages = userGallery.GalleryOfImages.Select(g => new GalleryImageDocument(g.ImageId, g.ImageUrl)).ToList();
                await _repository.UpdateAsync(userGalleryDocument);
            }
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<GalleryImage>> GetGalleryImagesAsync(Guid userId)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userId);
            return userGalleryDocument?.GalleryOfImages.Select(g => new GalleryImage(g.ImageId, g.ImageUrl, g.DateAdded)) ?? Enumerable.Empty<GalleryImage>();
        }

        public async Task AddGalleryImageAsync(Guid userId, GalleryImage galleryImage)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userId);

            if (userGalleryDocument == null)
            {
                userGalleryDocument = new UserGalleryDocument
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    GalleryOfImages = new List<GalleryImageDocument>
                    {
                        new GalleryImageDocument(galleryImage.ImageId, galleryImage.ImageUrl)
                    }
                };
                await _repository.AddAsync(userGalleryDocument);
            }
            else
            {
                var galleryImages = userGalleryDocument.GalleryOfImages.ToList();
                galleryImages.Add(new GalleryImageDocument(galleryImage.ImageId, galleryImage.ImageUrl));
                userGalleryDocument.GalleryOfImages = galleryImages;
                await _repository.UpdateAsync(userGalleryDocument);
            }
        }

         public async Task RemoveGalleryImageAsync(Guid userId, Guid imageId)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userId);

            if (userGalleryDocument != null)
            {
                var galleryImages = userGalleryDocument.GalleryOfImages.Where(g => g.ImageId != imageId).ToList();
                userGalleryDocument.GalleryOfImages = galleryImages;
                await _repository.UpdateAsync(userGalleryDocument);
            }
        }

        public async Task RemoveGalleryImageByUrlAsync(Guid userId, string imageUrl)
        {
            var userGalleryDocument = await _repository.GetAsync(x => x.UserId == userId);

            if (userGalleryDocument != null)
            {
                var galleryImages = userGalleryDocument.GalleryOfImages.Where(g => g.ImageUrl != imageUrl).ToList();
                userGalleryDocument.GalleryOfImages = galleryImages;
                await _repository.UpdateAsync(userGalleryDocument);
            }
        }
    }
}
