using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.Repository
{
    public class RoleRepository : BaseRepository<IdentityRole>
    {
        public RoleRepository(Context context)
            :base(context)
        {

        }
        public IdentityRole GetRoleById(string entityId)
        {
            return Context.Roles
                .Where(r => r.Id == entityId)
                .SingleOrDefault();
        }

        public override IList<IdentityRole> GetList()
        {
            throw new NotImplementedException();
        }

        public override IdentityRole Get(int? entityId)
        {
            throw new NotImplementedException();
        }
    }
}