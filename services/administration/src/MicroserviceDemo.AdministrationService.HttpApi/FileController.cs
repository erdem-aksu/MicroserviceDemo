using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.Blob;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace MicroserviceDemo.AdministrationService;

[Area("File")]
[RemoteService(Name = AdministrationServiceRemoteServiceConsts.RemoteServiceName)]
[Route("api/file")]
public class FileController : AdministrationServiceController, IFileAppService
{
    private readonly IFileAppService _fileAppService;

    public FileController(IFileAppService fileAppService)
    {
        _fileAppService = fileAppService;
    }

    [HttpPost]
    public Task SaveBlobAsync(SaveBlobInputDto input)
    {
        return _fileAppService.SaveBlobAsync(input);
    }

    [HttpGet]
    public Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
    {
        return _fileAppService.GetBlobAsync(input);
    }
}