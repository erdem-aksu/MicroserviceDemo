using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using MicroserviceDemo.ReportService.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo.ReportService;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class ReportServiceDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(
            options => { options.FileSets.AddEmbedded<ReportServiceDomainSharedModule>(); }
        );

        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Resources
                    .Add<ReportServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/ReportService");
            }
        );

        Configure<AbpExceptionLocalizationOptions>(
            options => { options.MapCodeNamespace("ReportService", typeof(ReportServiceResource)); }
        );
    }
}