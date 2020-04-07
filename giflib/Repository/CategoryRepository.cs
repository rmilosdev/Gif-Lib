using giflib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace giflib.Repository
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(Context context)
            : base(context)
        {
            
        }

        public override Category Get(int? categoryId)
        {
            return Context.Categories.Where(cb => cb.Id == categoryId)
                    .SingleOrDefault();
        }

        public override IList<Category> GetList()
        {
            return Context.Categories
                .OrderBy(c => c.Name)
                .ToList();
        }
        public void Update1(Category category)
        {
            Context.Set<Category>().AddOrUpdate(category);
        }

        public Category checkForUsername(string categoryName)
        {
            return Context.Categories.
                Where(c => c.Name == categoryName)
                .SingleOrDefault();
        }
    }
}