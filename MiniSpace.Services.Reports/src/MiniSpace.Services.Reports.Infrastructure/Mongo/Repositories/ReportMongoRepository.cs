using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;

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
                && r.Status == ReportStatus.Submitted || r.Status == ReportStatus.UnderReview);

            return reports.Select(r => r.AsEntity());
        }

        public Task AddAsync(Report report)
            => _repository.AddAsync(report.AsDocument());

        public Task UpdateAsync(Report report)
            => _repository.UpdateAsync(report.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}