using System;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MicroserviceDemo.AdministrationService.Blob;
using MicroserviceDemo.ContactService.Contacts;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace MicroserviceDemo.ReportService.Reports;

public class ReportRequestedEventHandler : IDistributedEventHandler<ReportRequestedEto>, ITransientDependency
{
    private readonly ReportManager _reportManager;
    private readonly IContactAppService _contactAppService;
    private readonly IFileAppService _fileAppService;
    private readonly ILogger<ReportRequestedEventHandler> _logger;

    public ReportRequestedEventHandler(
        ReportManager reportManager,
        IContactAppService contactAppService,
        IFileAppService fileAppService,
        ILogger<ReportRequestedEventHandler> logger)
    {
        _reportManager = reportManager;
        _contactAppService = contactAppService;
        _fileAppService = fileAppService;
        _logger = logger;
    }

    [UnitOfWork]
    public async Task HandleEventAsync(ReportRequestedEto eventData)
    {
        if (eventData == null)
        {
            _logger.LogWarning("Handling ReportRequestedEvent failed! No user information found!");
            return;
        }

        var report = await _reportManager.FindAsync(eventData.ReportId);

        if (report?.Status != ReportStatus.Pending)
        {
            _logger.LogWarning("Handling ReportRequestedEvent failed! No report found with id: {ReportId}", eventData.ReportId);
            return;
        }

        try
        {
            var contactStats = await _contactAppService.GetReportAsync(
                new GetContactsReportInput
                {
                    Location = eventData.Location
                }
            );

            using var memoryStream = new MemoryStream();
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Contacts Report");
            worksheet.Cell(1, 1).Value = "Location";
            worksheet.Cell(1, 2).Value = "Total Contacts";
            worksheet.Cell(1, 3).Value = "Total Phone Numbers";

            worksheet.Cell(2, 1).Value = contactStats.Location;
            worksheet.Cell(2, 2).Value = contactStats.ContactCount;
            worksheet.Cell(2, 3).Value = contactStats.ContactPhoneCount;

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(memoryStream);

            var fileName = $"ContactsReport_{Guid.NewGuid():N}.xlsx";

            await _fileAppService.SaveBlobAsync(
                new SaveBlobInputDto
                {
                    Content = memoryStream.ToArray(),
                    Name = fileName,
                }
            );

            report.Status = ReportStatus.Completed;
            report.Name = fileName;

            await _reportManager.UpdateAsync(report);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Handling ReportRequestedEvent failed! ReportId: {ReportId}", eventData.ReportId);

            report.Status = ReportStatus.Failed;

            await _reportManager.UpdateAsync(report);
        }
    }
}