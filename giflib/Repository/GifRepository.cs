using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace giflib.Repository
{
    public class GifRepository : BaseRepository<Gif>
    {
        public GifRepository(Context context)
            :base(context)
        {

        }

        public override Gif Get(int? entityId)
        {
            return Context.Gifs
                .Include(gf => gf.User)
                .Include(gf => gf.Category)
                .Where(gf => gf.Id == entityId)
                .SingleOrDefault();
        }

        public override IList<Gif> GetList()
        {
            return Context.Gifs
                .OrderBy(g => g.Description)
                .ToList();
        }

        public List<Gif> GetGifsByCategory(int categoryId)
        {
            return Context.Gifs.
                Include(g => g.Category)
                .Where(g => g.Category.Id == categoryId)
                .ToList();
        }


        public List<Gif> GetMyList(string userId)
        {
            return Context.Gifs
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.DateUploaded)
                .ToList();
        }
    }
}