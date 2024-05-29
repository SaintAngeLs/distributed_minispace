using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MongoDB.Bson;
using MongoDB.Driver;


namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentsHandler : IQueryHandler<GetStudents, Application.Queries.PagedResult<StudentDto>>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;
         private const string BaseUrl = "students"; 

        public GetStudentsHandler(IMongoRepository<StudentDocument, Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task<Application.Queries.PagedResult<StudentDto>> HandleAsync(GetStudents query, CancellationToken cancellationToken)
        {
            var filter = Builders<StudentDocument>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                var regexFirstName = new BsonRegularExpression(query.FirstName, "i");
                filter &= Builders<StudentDocument>.Filter.Regex(x => x.FirstName, regexFirstName);
            }
            if(!string.IsNullOrWhiteSpace(query.LastName))
            {
                var regexLastName = new BsonRegularExpression(query.LastName, "i");
                filter &= Builders<StudentDocument>.Filter.Regex(x => x.LastName, regexLastName);
            }
            var options = new FindOptions<StudentDocument, StudentDocument>
            {
                Limit = query.ResultsPerPage,
                Skip = (query.Page - 1) * query.ResultsPerPage
            };

            using (var cursor = await _studentRepository.Collection.FindAsync(filter, options, cancellationToken))
            {
                var documents = await cursor.ToListAsync(cancellationToken);
                var dtos = documents.Select(s => s.AsDto()).ToList();
                var total = await _studentRepository.Collection.CountDocumentsAsync(filter);
                return new Application.Queries.PagedResult<StudentDto>(dtos, (int)total, query.ResultsPerPage, query.Page, BaseUrl);
            }
        }

    }
}    

