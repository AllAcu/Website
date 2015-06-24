using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using AllAcu.Models;
using AllAcu.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Pocket;

namespace AllAcu
{
    using AuthenticateDelegate = Func<string[], Action<IIdentity, IDictionary<string, string>, IDictionary<string, object>, object>, object, Task>;

    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        internal void ConfigureAuth(IAppBuilder app, PocketContainer container)
        {
            container.Register<IUserStore<ApplicationUser>>(c => new UserStore<ApplicationUser>(c.Resolve<AuthorizationDbContext>()));
            container.Register<IDataSerializer<AuthenticationTicket>>(c => c.Resolve<TicketSerializer>());
            container.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider());
            //container.Register<ISecureDataFormat<AuthenticationTicket>>(c =>
            //    new SecureDataFormat<AuthenticationTicket>(
            //        c.Resolve<IDataSerializer<AuthenticationTicket>>(),
            //        c.Resolve<IDataProtectionProvider>().Create(),
            //        new Base64TextEncoder()));

            app.CreatePerOwinContext(() => container.Resolve<AuthorizationDbContext>());
            app.CreatePerOwinContext<ApplicationUserManager>((o, c) => container.Resolve<ApplicationUserManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            ApplicationOAuthProvider.PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = container.Resolve<ApplicationOAuthProvider>(),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
