using giflib.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace giflib.Repository
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gif> Gifs { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public Context()
            :base("Context")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Removing the pluralizing table name convention 
            // so our table names will use our entity class singular names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}