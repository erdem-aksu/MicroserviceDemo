using FluentValidation;
using MicroserviceDemo.ContactService.Validation;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.ContactService.Contacts;

public class ContactCreateOrUpdateDtoValidator : FluentValidatorBase<ContactCreateOrUpdateDtoBase>
{
    public ContactCreateOrUpdateDtoValidator(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        RuleFor(t => t.Name).NotEmpty().MaximumLength(ContactConsts.NameMaxLength).WithName(L["DisplayName:Name"]);
        RuleFor(t => t.SurName).NotEmpty().MaximumLength(ContactConsts.NameMaxLength).WithName(L["DisplayName:SurName"]);
        RuleFor(t => t.Company).NotEmpty().MaximumLength(ContactConsts.NameMaxLength).WithName(L["DisplayName:Company"]);

        RuleForEach(t => t.Info).SetValidator(new ContactInfoCreateOrUpdateDtoValidator(LazyServiceProvider));
    }
}