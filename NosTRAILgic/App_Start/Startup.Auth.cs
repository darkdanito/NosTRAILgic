using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace NosTRAILgic
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            ////Facebook Local Host
            //app.UseFacebookAuthentication(
            //   appId: "797670433702310",
            //   appSecret: "fa3dfbef8d5fb8b18ab8c12e704fd687");

            //Facebook Azure Host
            app.UseFacebookAuthentication(
               appId: "797670433702310",
               appSecret: "e693642fcff48c361ca1002172130d4c");

            //Google Azure & Local Host
            app.UseGoogleAuthentication(
               clientId: "822558733343-h82u9otfbvuppf4t16faqo5iqp95r0b8.apps.googleusercontent.com",
               clientSecret: "AzjScOnOPdnku4FATGbQX3-q");
        }
    }
}