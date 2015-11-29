using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;

namespace AllAcu.Authentication
{
    public class CareProviderIdFilter : ActionFilterAttribute
    {
        public const string providerCookieName = "CareProviderId";

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cookie = context.ActionArguments.ContainsKey(providerCookieName) ? context.ActionArguments[providerCookieName] : null;

            context.ActionArguments[providerCookieName] = cookie != null
                ? (object) Guid.Parse(cookie.ToString())
                : null;
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
