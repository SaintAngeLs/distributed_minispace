using System.Text.RegularExpressions;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
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
            
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                string searchTerm = query.Name.Trim();

                var parts = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var filters = new List<FilterDefinition<StudentDocument>>();

                if (parts.Length == 1)
                {
                    var regex = new BsonRegularExpression(new Regex(parts[0], RegexOptions.IgnoreCase));
                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.FirstName, regex));
                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.LastName, regex));
                }
                else if (parts.Length >= 2)
                {
                    var firstNameRegex = new BsonRegularExpression(new Regex(parts[0], RegexOptions.IgnoreCase));
                    var lastNameRegex = new BsonRegularExpression(new Regex(parts[1], RegexOptions.IgnoreCase));

                    filters.Add(Builders<StudentDocument>.Filter.And(
                        Builders<StudentDocument>.Filter.Regex(x => x.FirstName, firstNameRegex),
                        Builders<StudentDocument>.Filter.Regex(x => x.LastName, lastNameRegex)
                    ));
        
                    filters.Add(Builders<StudentDocument>.Filter.And(
                        Builders<StudentDocument>.Filter.Regex(x => x.FirstName, lastNameRegex),
                        Builders<StudentDocument>.Filter.Regex(x => x.LastName, firstNameRegex)
                    ));

                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.FirstName, firstNameRegex));
                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.LastName, firstNameRegex));
                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.FirstName, lastNameRegex));
                    filters.Add(Builders<StudentDocument>.Filter.Regex(x => x.LastName, lastNameRegex));
                }

                filter &= Builders<StudentDocument>.Filter.Or(filters);
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

