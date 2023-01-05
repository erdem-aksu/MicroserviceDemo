using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroserviceDemo.ReportService.Reports;

public interface IReportAppService : IApplicationService
{
    Task<ReportDto> GetAsync(Guid id);

    Task<PagedResultDto<ReportListDto>> GetListAsync(GetReportsInput input);

    Task<ReportDto> CreateAsync(ReportCreateDto input);

    Task DeleteAsync(Guid id);
}