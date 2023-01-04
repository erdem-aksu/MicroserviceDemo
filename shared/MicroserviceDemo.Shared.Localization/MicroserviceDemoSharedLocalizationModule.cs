using MicroserviceDemo.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace MicroserviceDemo;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class MicroserviceDemoSharedLocalizationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(
            options => { options.FileSets.AddEmbedded<MicroserviceDemoSharedLocalizationModule>(); }
        );

        Configure<AbpLocalizationOptions>(
            options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English", "gb"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "tr"));

                options.Resources
                    .Add<MicroserviceDemoResource>("en")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    )
                    .AddVirtualJson("/Localization/MicroserviceDemo");

                options.DefaultResourceType = typeof(MicroserviceDemoResource);
            }
        );
    }
}