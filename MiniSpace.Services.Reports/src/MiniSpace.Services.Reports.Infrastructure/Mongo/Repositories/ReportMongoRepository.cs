using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public class ReportMongoRepository : IReportRepository
    {
        private readonly IMongoRepository<ReportDocument, Guid> _repository;

        public ReportMongoRepository(IMongoRepository<ReportDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Report> GetAsync(Guid id)
        {
            var report = await _repository.GetAsync(r => r.Id == id);

            return report?.AsEntity();
        }
        
        public async Task<IEnumerable<Report>> GetStudentActiveReportsAsync(Guid studentId)
        {
            var reports = await _repository.FindAsync(r => r.IssuerId == studentId 
                && r.State == ReportState.Submitted || r.State == ReportState.UnderReview);

            return reports.Select(r => r.AsEntity());
        }

        public Task AddAsync(Report report)
            => _repository.AddAsync(report.AsDocument());

        public Task UpdateAsync(Report report)
            => _repository.UpdateAsync(report.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
        
        private async Task<(int totalPages, int totalElements, IEnumerable<ReportDocument> data)> BrowseAsync(
            FilterDefinition<ReportDocument> filterDefinition, SortDefinition<ReportDocument> sortDefinition, 
            int pageNumber, int pageSize)
        {
            var pagedEvents = await _repository.Collection.AggregateByPage(
                filterDefinition,
                sortDefinition,
                pageNumber,
                pageSize);

            return pagedEvents;
        }
        
        public async Task<(IEnumerable<Report> posts, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseReportsAsync(int pageNumber, int pageSize, 
            IEnumerable<string> sortBy, string direction)
        {
            var filterDefinition = Extensions.ToFilterDefinition();
            var sortDefinition = Extensions.ToSortDefinition(sortBy, direction);
            
            var pagedEvents = await BrowseAsync(filterDefinition, sortDefinition, pageNumber, pageSize);
            
            return (pagedEvents.data.Select(r => r.AsEntity()), pageNumber, pageSize,
                pagedEvents.totalPages, pagedEvents.totalElements);
        }
    }    
}