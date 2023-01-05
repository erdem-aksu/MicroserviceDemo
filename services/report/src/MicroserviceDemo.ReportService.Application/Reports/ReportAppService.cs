using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MicroserviceDemo.ReportService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;

namespace MicroserviceDemo.ReportService.Reports;

[Authorize(ReportServicePermissions.Reports.Default)]
public class ReportAppService : ReportServiceAppService, IReportAppService
{
    protected ReportManager ReportManager { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    public ReportAppService(ReportManager reportManager, IDistributedEventBus distributedEventBus)
    {
        ReportManager = reportManager;
        DistributedEventBus = distributedEventBus;
    }

    public async Task<ReportDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Report, ReportDto>(
            await ReportManager.GetAsync(id)
        );
    }

    public async Task<PagedResultDto<ReportListDto>> GetListAsync(GetReportsInput input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ReportListDto.CreationTime) + " desc";
        }

        var query = (await ReportManager.GetQueryableAsync())
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u => u.Name.Contains(input.Filter)
            );

        var count = await AsyncExecuter.CountAsync(query);
        var list = await AsyncExecuter.ToListAsync(
            query.ProjectTo<ReportListDto>(ObjectMapper.GetMapper().ConfigurationProvider)
                .OrderBy(input.Sorting)
                .PageBy(input.SkipCount, input.MaxResultCount)
        );

        return new PagedResultDto<ReportListDto>(count, list);
    }

    [Authorize(ReportServicePermissions.Reports.Create)]
    public async Task<ReportDto> CreateAsync(ReportCreateDto input)
    {
        var report = ReportManager.Create();
        input.MapExtraPropertiesTo(report);

        await ReportManager.InsertAsync(report);

        await CurrentUnitOfWork.SaveChangesAsync();

        await DistributedEventBus.PublishAsync(
            new ReportRequestedEto
            {
                ReportId = report.Id,
                Location = input.Location
            }
        );

        return ObjectMapper.Map<Report, ReportDto>(report);
    }

    [Authorize(ReportServicePermissions.Reports.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await ReportManager.DeleteAsync(id);
    }
}