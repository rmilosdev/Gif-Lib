using giflib.Models;
using giflib.Repository;
using giflib.Security;
using giflib.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace giflib.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authManager;

        private UserRepository _userRepository = null;
        private GifRepository _gifRepository = null;
        private FavoriteRepository _favoriteRepository = null;
        private RoleRepository _roleRepository = null;
        public AccountController(ApplicationUserManager userManager,
                                 ApplicationSignInManager signInManager,
                                 IAuthenticationManager authManager,
                                 UserRepository userRepository,
                                 GifRepository gifRepository,
                                 FavoriteRepository favoriteRepository,
                                 RoleRepository roleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authManager = authManager;
            _userRepository = userRepository;
            _gifRepository = gifRepository;
            _favoriteRepository = favoriteRepository;
            _roleRepository = roleRepository;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _userManager.FindByNameAsync(viewModel.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", $"The provided email address '{viewModel.Username}' has already been used to register an account. Please sign-in using your existing account.");
                }

                var user = new User {
                    UserName = viewModel.Username
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);
                

                if (result.Succeeded)
                {
                    //Dodeliti useru role User ili Admin 
                    await _userManager.AddToRoleAsync(user.Id, _roleRepository.GetRoleById("2").Name);

                    await _signInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    return RedirectToAction("Index", "Gif");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(AccountSignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Username,
                                                                  viewModel.Password,
                                                                  viewModel.RememberMe,
                                                                  false);
            switch (result) {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Gif");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(viewModel);
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException("Identity feature not implemented.");
                default:
                    throw new NotImplementedException("Unexpected Microsoft.AspNet.Identity.Owin.SignInStatus enum value: " + result);
            }                                                    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Gif");
        }

        public ActionResult MyList()
        {
            User currentUser = _userRepository.GetUser(User.Identity.Name);

            var usersGifs = _gifRepository.GetMyList(currentUser.Id);
            //pocetak ponovljenog koda - Kod je nastao U action metodi Index GifControllera
            var userFavorites = _favoriteRepository.GetUserList(currentUser.Id);
            var listOfFavouriteGifs = new List<Gif>();

            foreach (var fav in userFavorites)
            {
                var gif = _gifRepository.Get(fav.Gif.Id);
                listOfFavouriteGifs.Add(gif);
            }
            //kraj ponovljenog koda
            foreach (var gif in usersGifs)
            {
                if (listOfFavouriteGifs.Contains(gif))
                {
                    gif.Favorite = true;
                }
                else
                {
                    gif.Favorite = false;
                }
            }

            return View(usersGifs);
        }

        public ActionResult Users()
        {
            if(User.IsInRole("Admin") == false)
            {
                return HttpNotFound();
            }

            var allUsers = _userRepository.GetList();
            var usersToReturn = new List<User>();

            foreach(var user in allUsers)
            {
                if(_userManager.IsInRole(user.Id, "User"))
                {
                    usersToReturn.Add(user);
                }
            }

            return View(usersToReturn);
        }

        public ActionResult Delete(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = _userRepository.GetById(userId);

            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteUser(string userId)
        {

            var usersGifs = _gifRepository.GetMyList(userId);
            var favoriteGifs = _favoriteRepository.GetUserList(userId);

            if(usersGifs != null)
            {
                if(favoriteGifs != null)
                {
                    foreach(var fav in favoriteGifs)
                    {
                        _favoriteRepository.Delete(fav.Id);
                    }
                }
                foreach(var gif in usersGifs)
                {
                    _gifRepository.Delete(gif.Id);
                }
            }

            _userRepository.DeleteUser(userId);

            TempData["Message"] = "Your User was successfully deleted!";

            return RedirectToAction("Index", "Gif");
        }

    }
}