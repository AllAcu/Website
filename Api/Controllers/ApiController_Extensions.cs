using System;
using System.Web.Http;

namespace AllAcu.Controllers
{
    public static class ApiController_Extensions
    {
        public const string providerCookieName = "CareProviderId";

        public static Guid? CurrentProviderId(this ApiController controller)
        {
            var actionArguments = controller.ActionContext.ActionArguments;
            return actionArguments[providerCookieName] != null ? Guid.Parse(actionArguments[providerCookieName].ToString()) : (Guid?)null;
        }
    }
}
