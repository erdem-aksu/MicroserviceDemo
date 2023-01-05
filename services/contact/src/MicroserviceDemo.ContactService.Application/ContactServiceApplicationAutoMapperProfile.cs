using AutoMapper;
using MicroserviceDemo.ContactService.Contacts;

namespace MicroserviceDemo.ContactService;

public class ContactServiceApplicationAutoMapperProfile : Profile
{
    public ContactServiceApplicationAutoMapperProfile()
    {
        CreateMap<Contact, ContactDto>().MapExtraProperties();
        CreateMap<Contact, ContactListDto>();
        CreateMap<ContactInfo, ContactInfoDto>();
    }
}