using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace MicroserviceDemo.ReportService.Reports
{
    public class ReportManager : DomainService
    {
        private IRepository<Report, Guid> ReportRepository { get; }

        public ReportManager(IRepository<Report, Guid> reportRepository)
        {
            ReportRepository = reportRepository;
        }

        public Task<IQueryable<Report>> GetQueryableAsync() => ReportRepository.GetQueryableAsync();

        public Task<IQueryable<Report>> WithDetailsAsync() => ReportRepository.WithDetailsAsync();

        public Task<IQueryable<Report>> WithDetailsAsync(params Expression<Func<Report, object>>[] propertySelectors) => ReportRepository.WithDetailsAsync(propertySelectors);

        public Report Create()
        {
            return new Report(GuidGenerator.Create());
        }

        [UnitOfWork]
        public async Task<Report> InsertAsync(Report report, bool autoSave = false)
        {
            return await ReportRepository.InsertAsync(report, autoSave);
        }

        [UnitOfWork]
        public async Task<Report> UpdateAsync(Report report, bool autoSave = false)
        {
            return await ReportRepository.UpdateAsync(report, autoSave);
        }

        [UnitOfWork]
        public async Task DeleteAsync(Guid id, bool autoSave = false)
        {
            var report = await GetAsync(id, false);

            await ReportRepository.DeleteAsync(report, autoSave);
        }

        public async Task<Report> GetAsync(Guid id, bool includeDetails = true)
        {
            return await ReportRepository.GetAsync(id, includeDetails);
        }

        public async Task<Report> GetAsync(Guid id, params Expression<Func<Report, object>>[] propertySelectors)
        {
            var report = await AsyncExecuter.SingleOrDefaultAsync(
                await ReportRepository.WithDetailsAsync(propertySelectors),
                c => c.Id == id
            );

            if (report == null)
            {
                throw new EntityNotFoundException(typeof(Report));
            }

            return report;
        }

        public async Task<Report> FindAsync(Guid id, bool includeDetails = true)
        {
            return await ReportRepository.FindAsync(id, includeDetails);
        }

        public async Task<Report> FindAsync(Expression<Func<Report, bool>> predicate, bool includeDetails = true)
        {
            return await ReportRepository.FindAsync(predicate, includeDetails);
        }
    }
}