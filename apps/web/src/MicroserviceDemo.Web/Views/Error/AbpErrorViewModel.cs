using Volo.Abp.Http;

namespace MicroserviceDemo.Web.Views.Error
{
    public class AbpErrorViewModel
    {
        public RemoteServiceErrorInfo ErrorInfo { get; set; }

        public int HttpStatusCode { get; set; }
    }
}
