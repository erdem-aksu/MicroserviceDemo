using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MicroserviceDemo.AdministrationService.Blob;

public interface IFileAppService : IApplicationService
{
    Task SaveBlobAsync(SaveBlobInputDto input);

    Task<BlobDto> GetBlobAsync(GetBlobRequestDto input);
}
