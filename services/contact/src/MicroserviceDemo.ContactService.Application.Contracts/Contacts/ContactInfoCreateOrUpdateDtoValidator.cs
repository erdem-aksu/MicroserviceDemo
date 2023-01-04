using FluentValidation;
using MicroserviceDemo.ContactService.Validation;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactInfoCreateOrUpdateDtoValidator : FluentValidatorBase<ContactInfoCreateOrUpdateDto>
{
    public ContactInfoCreateOrUpdateDtoValidator(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        RuleFor(t => t.Type).IsInEnum().WithName("DisplayName:ContactInfoType");
        RuleFor(t => t.Value).NotEmpty().MaximumLength(ContactConsts.InfoMaxLength).WithName("DisplayName:ContactInfoValue");
    }
}