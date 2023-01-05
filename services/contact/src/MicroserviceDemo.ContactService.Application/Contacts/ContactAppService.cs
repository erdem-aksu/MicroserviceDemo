using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MicroserviceDemo.ContactService.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;

namespace MicroserviceDemo.ContactService.Contacts;

[Authorize(ContactServicePermissions.Contacts.Default)]
public class ContactAppService : ContactServiceAppService, IContactAppService
{
    protected ContactManager ContactManager { get; }

    public ContactAppService(ContactManager contactManager)
    {
        ContactManager = contactManager;
    }

    public async Task<ContactDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Contact, ContactDto>(
            await ContactManager.GetAsync(id, c => c.Info)
        );
    }

    public async Task<PagedResultDto<ContactListDto>> GetListAsync(GetContactsInput input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(ContactListDto.CreationTime) + " desc";
        }

        var query = (await ContactManager.WithDetailsAsync(c => c.Info))
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u => u.Name.Contains(input.Filter)
            )
            .WhereIf(!input.Location.IsNullOrWhiteSpace(), v => v.Info.Any(i => i.Type == ContactInfoType.Location && i.Value.Contains(input.Location)));

        var count = await AsyncExecuter.CountAsync(query);
        var list = await AsyncExecuter.ToListAsync(
            query.ProjectTo<ContactListDto>(ObjectMapper.GetMapper().ConfigurationProvider)
                .OrderBy(input.Sorting)
                .PageBy(input.SkipCount, input.MaxResultCount)
        );

        return new PagedResultDto<ContactListDto>(count, list);
    }

    public async Task<ContactReportDto> GetReportAsync(GetContactsReportInput input)
    {
        var contacts = await AsyncExecuter.ToListAsync(
            (await ContactManager.WithDetailsAsync(c => c.Info))
            .Where(c => c.Info.Any(i => i.Type == ContactInfoType.Location && i.Value.Contains(input.Location)))
            .Select(
                c => new
                {
                    PhoneCount = c.Info.Count(i => i.Type == ContactInfoType.Phone),
                }
            )
        );

        return new ContactReportDto
        {
            Location = input.Location,
            ContactCount = contacts.Count,
            ContactPhoneCount = contacts.Sum(c => c.PhoneCount)
        };
    }

    [Authorize(ContactServicePermissions.Contacts.Create)]
    public async Task<ContactDto> CreateAsync(ContactCreateDto input)
    {
        var contact = ContactManager.Create(
            input.Name,
            input.SurName,
            input.Company
        );
        input.MapExtraPropertiesTo(contact);

        foreach (var contactInfo in input.Info)
        {
            contact.Info.Add(
                new ContactInfo(
                    GuidGenerator.Create(),
                    contact.Id,
                    contactInfo.Type,
                    contactInfo.Value
                )
            );
        }

        await ContactManager.InsertAsync(contact);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Contact, ContactDto>(contact);
    }

    [Authorize(ContactServicePermissions.Contacts.Update)]
    public async Task<ContactDto> UpdateAsync(Guid id, ContactUpdateDto input)
    {
        var contact = await ContactManager.GetAsync(id, c => c.Info);

        contact.Name = input.Name;
        contact.SurName = input.SurName;
        contact.Company = input.Company;

        contact.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(contact);

        contact.Info.Clear();

        foreach (var contactInfo in input.Info)
        {
            contact.Info.Add(
                new ContactInfo(
                    GuidGenerator.Create(),
                    contact.Id,
                    contactInfo.Type,
                    contactInfo.Value
                )
            );
        }

        await ContactManager.UpdateAsync(contact);

        return ObjectMapper.Map<Contact, ContactDto>(contact);
    }

    [Authorize(ContactServicePermissions.Contacts.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await ContactManager.DeleteAsync(id);
    }
}