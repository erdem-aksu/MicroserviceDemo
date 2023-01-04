using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroserviceDemo.Web.Bundling
{
    public class MicroserviceDemoWebStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/css/custom.css");
        }
    }
}