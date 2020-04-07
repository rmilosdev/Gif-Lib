using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Favorites = new List<Favorite>();
        }

        public List<Favorite> Favorites { get; set; }

    }
}