// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using MicroserviceDemo.ContactService.Contacts;

// ReSharper disable once CheckNamespace
namespace MicroserviceDemo.ContactService.ClientProxies;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IContactAppService), typeof(ContactClientProxy))]
public partial class ContactClientProxy : ClientProxyBase<IContactAppService>, IContactAppService
{
    public virtual async Task<ContactDto> GetAsync(Guid id)
    {
        return await RequestAsync<ContactDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<PagedResultDto<ContactListDto>> GetListAsync(GetContactsInput input)
    {
        return await RequestAsync<PagedResultDto<ContactListDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetContactsInput), input }
        });
    }

    public virtual async Task<ContactReportDto> GetReportAsync(GetContactsReportInput input)
    {
        return await RequestAsync<ContactReportDto>(nameof(GetReportAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetContactsReportInput), input }
        });
    }

    public virtual async Task<ContactDto> CreateAsync(ContactCreateDto input)
    {
        return await RequestAsync<ContactDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(ContactCreateDto), input }
        });
    }

    public virtual async Task<ContactDto> UpdateAsync(Guid id, ContactUpdateDto input)
    {
        return await RequestAsync<ContactDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id },
            { typeof(ContactUpdateDto), input }
        });
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }
}
