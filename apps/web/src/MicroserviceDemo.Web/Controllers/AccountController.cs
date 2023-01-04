using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Authentication;

namespace MicroserviceDemo.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : ChallengeAccountController
    {
    }
}