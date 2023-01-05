using System.Threading.Tasks;
using MicroserviceDemo.AdministrationService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace MicroserviceDemo.AdministrationService.Blob;

[Authorize(AdministrationServicePermissions.Identity.Users.Default)]
public class FileAppService : ApplicationService, IFileAppService
{
    private readonly IBlobContainer _fileContainer;

    public FileAppService(IBlobContainer fileContainer)
    {
        _fileContainer = fileContainer;
    }

    public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
    {
        var blob = await _fileContainer.GetAllBytesAsync(input.Name);
        return new BlobDto { Name = input.Name, Content = blob };
    }

    public async Task SaveBlobAsync(SaveBlobInputDto input)
    {
        await _fileContainer.SaveAsync(input.Name, input.Content, true);
    }
}