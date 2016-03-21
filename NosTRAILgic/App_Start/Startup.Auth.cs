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

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //Facebook Local host
            app.UseFacebookAuthentication(
               appId: "494623630721091",
               appSecret: "5dced5c48305726f4665dadd1a70277");
            //Azure Azure
            //app.UseFacebookAuthentication(
            //   appId: "228561424165052",
            //   appSecret: "2d43d7af009a35d13921102a951852f");

            //Google Local host
            app.UseGoogleAuthentication(
                clientId: "905341464171-iimnv742if6bjg80unlakipuh1tq32lj.apps.googleusercontent.com",
                clientSecret: "4jNBKHe0T2jK3IgJ73mYVl2v");
            //Google Azure
            //app.UseGoogleAuthentication(
            //    clientId: "744133716115-25rvvam56mltmb10m0jn3slg4t3uum2u.apps.googleusercontent.com",
            //    clientSecret: "Pi5mX23y8eeaIT1Z0m4gI");
        }
    }
}