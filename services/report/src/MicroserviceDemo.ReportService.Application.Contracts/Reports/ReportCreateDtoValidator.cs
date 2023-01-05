using FluentValidation;
using MicroserviceDemo.ReportService.Validation;
using Volo.Abp.DependencyInjection;

namespace MicroserviceDemo.ReportService.Reports;

public class ReportCreateDtoValidator : FluentValidatorBase<ReportCreateDto>
{
    public ReportCreateDtoValidator(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
        RuleFor(t => t.Location).NotEmpty().WithName(L["DisplayName:Location"]);
    }
}