using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib
{
    public partial class Startup
    {
        public string DefaultAuthentificationTypes { get; private set; }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/SignIn"),
                Provider = new CookieAuthenticationProvider(),
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true
            }); 
        }
    }
}