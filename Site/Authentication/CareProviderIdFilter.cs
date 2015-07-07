using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AllAcu.Authentication
{
    public class CareProviderIdFilter : ActionFilterAttribute
    {
        public const string providerCookieName = "CareProviderId";

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var cookie = actionContext.Request.Headers.GetCookies(providerCookieName).FirstOrDefault();

            actionContext.ActionArguments[providerCookieName] = cookie != null
                ? (object) Guid.Parse(cookie[providerCookieName].Value)
                : null;
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}
