using MicroserviceDemo.ContactService.Localization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo.ContactService;

[DependsOn(
    typeof(AbpFluentValidationModule)
)]
public class ContactServiceDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<ContactServiceDomainSharedModule>(); });

        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Resources
                    .Add<ContactServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/ContactService");
            }
        );

        Configure<AbpExceptionLocalizationOptions>(
            options => { options.MapCodeNamespace("ContactService", typeof(ContactServiceResource)); }
        );
    }
}