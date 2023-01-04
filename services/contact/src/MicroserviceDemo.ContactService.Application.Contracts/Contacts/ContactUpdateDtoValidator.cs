using MicroserviceDemo.ContactService.Validation;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactUpdateDtoValidator : FluentValidatorBase<ContactUpdateDto>
{
    public ContactUpdateDtoValidator(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        Include(new ContactCreateOrUpdateDtoValidator(lazyServiceProvider));
    }
}