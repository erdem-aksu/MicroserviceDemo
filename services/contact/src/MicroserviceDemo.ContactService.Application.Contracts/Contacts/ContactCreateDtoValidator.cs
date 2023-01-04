using MicroserviceDemo.ContactService.Validation;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactCreateDtoValidator : FluentValidatorBase<ContactCreateDto>
{
    public ContactCreateDtoValidator(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        Include(new ContactCreateOrUpdateDtoValidator(lazyServiceProvider));
    }
}