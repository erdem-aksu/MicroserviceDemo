using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroserviceDemo.Web.Bundling
{
    public class MicroserviceDemoWebScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            // context.Files.AddIfNotContains("/libs/jquery/jquery.js");
            // context.Files.AddIfNotContains("/libs/abp/utils/abp-utils.umd.min.js");
            // context.Files.AddIfNotContains("/libs/abp/core/abp.js");
            // context.Files.AddIfNotContains("/libs/abp/jquery/abp.jquery.js");
            // context.Files.AddIfNotContains("/libs/bootstrap/js/bootstrap.bundle.js");
            // context.Files.AddIfNotContains("/libs/simplebar/js/simplebar.min.js");
            // context.Files.AddIfNotContains("/js/nioapp.min.js");
            // context.Files.AddIfNotContains("/js/dashlite.js");
        }
    }
}