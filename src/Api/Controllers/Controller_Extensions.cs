using System;
using Microsoft.AspNet.Mvc;

namespace AllAcu.Controllers
{
    public static class Controller_Extensions
    {
        public const string providerCookieName = "CareProviderId";

        public static Guid? CurrentProviderId(this Controller controller)
        {
            var cookies = controller.ActionContext.HttpContext.Request.Cookies;
            return cookies.ContainsKey(providerCookieName) ? Guid.Parse(cookies[providerCookieName].ToString()) : (Guid?)null;
        }
    }
}
