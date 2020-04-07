using giflib.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giflib.Controllers
{
    public class RoleController : BaseController
    {
        private RoleRepository _roleRepository = null;

        public RoleController()
        {
            _roleRepository = new RoleRepository(Context);
        }

        // GET: All Roles
        public ActionResult Index()
        {
            var Roles = Context.Roles.ToList();
            return View(Roles);
        }

        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            _roleRepository.Add(role);
            return RedirectToAction("Index");
        }
    }
}