﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace NosTRAILgic
{
    public partial class Startup
    {
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

            //Facebook Azure Host
            app.UseFacebookAuthentication(
               appId: "219295651770203",
               appSecret: "b006fa0d4e67e0c39f67ee41fd4a5163");

            //Google Azure & Local Host
            app.UseGoogleAuthentication(
               clientId: "822558733343-h82u9otfbvuppf4t16faqo5iqp95r0b8.apps.googleusercontent.com",
               clientSecret: "AzjScOnOPdnku4FATGbQX3-q");
        }
    }
}