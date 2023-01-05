using System;
using System.Threading.Tasks;
using MicroserviceDemo.ContactService.Contacts;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace MicroserviceDemo.ContactService;

[Area(ContactServiceRemoteServiceConsts.ModuleName)]
[RemoteService(Name = ContactServiceRemoteServiceConsts.RemoteServiceName)]
[Route("api/contact")]
public class ContactController : ContactServiceController, IContactAppService
{
    private readonly IContactAppService _contactAppService;

    public ContactController(IContactAppService contactAppService)
    {
        _contactAppService = contactAppService;
    }

    [HttpGet("{id}")]
    public Task<ContactDto> GetAsync(Guid id)
    {
        return _contactAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ContactListDto>> GetListAsync(GetContactsInput input)
    {
        return _contactAppService.GetListAsync(input);
    }

    [HttpGet("report")]
    public Task<ContactReportDto> GetReportAsync(GetContactsReportInput input)
    {
        return _contactAppService.GetReportAsync(input);
    }

    [HttpPost]
    public Task<ContactDto> CreateAsync(ContactCreateDto input)
    {
        return _contactAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public Task<ContactDto> UpdateAsync(Guid id, ContactUpdateDto input)
    {
        return _contactAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _contactAppService.DeleteAsync(id);
    }
}