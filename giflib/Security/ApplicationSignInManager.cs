using giflib.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.Security
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager  applicationUserManager,
            IAuthenticationManager authenticationManager)
            :base(applicationUserManager, authenticationManager)
        {

        }
    }
}