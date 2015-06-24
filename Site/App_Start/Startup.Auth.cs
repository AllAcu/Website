using System;
using AllAcu.Models;
using AllAcu.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        internal void ConfigureAuth(IAppBuilder app, PocketContainer container)
        {
            container.Register<IUserStore<ApplicationUser>>(c => new UserStore<ApplicationUser>(c.Resolve<AuthorizationDbContext>()));
            container.Register<IDataSerializer<AuthenticationTicket>>(c => c.Resolve<TicketSerializer>());
            container.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider());

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                Provider = container.Resolve<ApplicationOAuthProvider>(),
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
