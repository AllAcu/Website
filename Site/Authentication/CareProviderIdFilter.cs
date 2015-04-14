using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Domain.Authentication
{
    public class CareProviderIdFilter : ActionFilterAttribute
    {
        public static readonly Guid HardCodedId = new Guid("949D90DD-8A4F-4466-B383-1A4B78468951");
        public const string providerCookieName = "CareProviderId";

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var cookie = actionContext.Request.Headers.GetCookies(providerCookieName).FirstOrDefault();

            actionContext.ActionArguments[providerCookieName] = cookie != null
                ? (object) Guid.Parse(cookie[providerCookieName].Value)
                : null;
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public static Guid GetCurrentProvider(HttpActionContext actionContext)
        {
            var id = actionContext.ActionArguments[providerCookieName];
            return id != null ? Guid.Parse(id.ToString()) : HardCodedId;
        }
    }
}
