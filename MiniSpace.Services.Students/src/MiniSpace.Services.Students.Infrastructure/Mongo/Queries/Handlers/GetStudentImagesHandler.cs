using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetStudentImagesHandler : IQueryHandler<GetStudentImages, StudentImagesDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;

        public GetStudentImagesHandler(IMongoRepository<StudentDocument, Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentImagesDto> HandleAsync(GetStudentImages query, CancellationToken cancellationToken)
        {
            var document = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
            if (document is null)
            {
                throw new StudentNotFoundException(query.StudentId);
            }

            var studentImages = new StudentImagesDto(
                document.Id,
                document.BannerUrl,
                document.GalleryOfImageUrls
            );

            return studentImages;       
        }
    }
}

