using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace giflib.Repository
{
    public class FavoriteRepository : BaseRepository<Favorite>
    {

        public FavoriteRepository(Context context)
        : base(context) {

        }
        public override Favorite Get(int? entityId)
        {
            throw new NotImplementedException();
        }

        public IList<Favorite> GetUserList(string userId)
        {
            return Context.Favorites
                .Include(fv => fv.Gif)
                .Include(fv => fv.User)
                .Where(fv => fv.User.Id == userId)
                .ToList();

        }

        public Favorite GetByGif(int gifId)
        {
            return Context.Favorites.
              Where(f => f.Gif.Id == gifId)
              .SingleOrDefault();
        }

        public Favorite CheckIfExists(string userId, int gifId)
        {
            return Context.Favorites.
                Where(f => f.User.Id == userId && f.Gif.Id == gifId)
                .SingleOrDefault();
        }

        public override IList<Favorite> GetList()
        {
            return Context.Favorites
                .OrderBy(f => f.Gif.Id)
                .ToList();
        }
    }
}