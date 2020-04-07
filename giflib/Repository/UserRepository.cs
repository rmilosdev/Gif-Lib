using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(Context context)
            :base(context)
        {
        }
        public User GetUser(string username)
        {
            return Context.Users
                .Where(u => u.UserName == username)
                .SingleOrDefault();
        }

        public override User Get(int? entityId)
        {
            throw new NotImplementedException();
        }


        public User GetById(string userId)
        {
            return Context.Users.
                Where(u => u.Id == userId)
                .SingleOrDefault();
        }

        public override IList<User> GetList()
        {
            return Context.Users
                 .OrderBy(u => u.UserName) 
                 .ToList();
        }

        //Metoda umesto Base Delete Metode zbog string parametera
        public void DeleteUser(string userId)
        {
            var set = Context.Set<User>();
            var entity = set.Find(userId);
            set.Remove(entity);

            Context.SaveChanges();
        }
    }
}