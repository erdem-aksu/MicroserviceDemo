using System;
using System.Threading.Tasks;
using MicroserviceDemo.ReportService.Reports;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ReportService;

[Area(ReportServiceRemoteServiceConsts.ModuleName)]
[RemoteService(Name = ReportServiceRemoteServiceConsts.RemoteServiceName)]
[Route("api/report")]
public class ReportController : ReportServiceController, IReportAppService
{
    private readonly IReportAppService _reportAppService;

    public ReportController(IReportAppService reportAppService)
    {
        _reportAppService = reportAppService;
    }

    [HttpGet("{id}")]
    public Task<ReportDto> GetAsync(Guid id)
    {
        return _reportAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ReportListDto>> GetListAsync(GetReportsInput input)
    {
        return _reportAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<ReportDto> CreateAsync(ReportCreateDto input)
    {
        return _reportAppService.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _reportAppService.DeleteAsync(id);
    }
}