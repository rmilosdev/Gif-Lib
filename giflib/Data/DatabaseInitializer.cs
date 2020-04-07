using giflib.Models;
using giflib.Repository;
using giflib.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace giflib.Data
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            base.Seed(context);

            //var userStore = new UserStore<User>(context);
            //var userManager = new ApplicationUserManager(userStore);

            //var userMilos = new User
            //{
            //    UserName = "milos14416"
            //};

            //userManager.Create(userMilos, "milos14416");
        }
    }
}