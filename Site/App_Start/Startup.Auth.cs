using System;
using AllAcu.Models;
using AllAcu.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
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

            container.Register(c => app.GetDataProtectionProvider());
            container.Register<ISecureDataFormat<AuthenticationTicket>>(c => c.Resolve<SecureDataFormat<AuthenticationTicket>>());

            app.CreatePerOwinContext(() => container.Resolve<AuthorizationDbContext>());
            app.CreatePerOwinContext<ApplicationUserManager>((o, c) => container.Resolve<ApplicationUserManager>());

            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

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
