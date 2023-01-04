using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroserviceDemo.ContactService.Contacts;

public interface IContactAppService : IApplicationService
{
    Task<ContactDto> GetAsync(Guid id);

    Task<PagedResultDto<ContactListDto>> GetListAsync(GetContactsInput input);

    Task<ContactDto> CreateAsync(ContactCreateDto input);

    Task<ContactDto> UpdateAsync(Guid id, ContactUpdateDto input);

    Task DeleteAsync(Guid id);
}