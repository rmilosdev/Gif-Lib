using giflib.Models;
using giflib.Repository;
using giflib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using giflib.Security;
using System.IO;

namespace giflib.Controllers
{
    public class GifController : BaseController
    {
        private GifRepository _gifRepository = null;
        private CategoryRepository _categoryRepository = null;
        private UserRepository _userRepository = null;
        private FavoriteRepository _favoriteRepository = null;
        private RoleRepository _roleRepository = null;

        private ApplicationUserManager _AppUserManager = null;
        public GifController(ApplicationUserManager userManager)
        {
            _gifRepository = new GifRepository(Context);
            _categoryRepository = new CategoryRepository(Context);
            _userRepository = new UserRepository(Context);
            _favoriteRepository = new FavoriteRepository(Context);
            _roleRepository = new RoleRepository(Context);
            _AppUserManager = userManager;
        }

        [AllowAnonymous]
       
        public ActionResult Index(string sreachInput)
        {
            var gifs = _gifRepository.GetList();

            if (sreachInput != null)
            {
                var listOfGifs = gifs.Where(g => g.Description.ToLower().Contains(sreachInput));

                if(listOfGifs.Count() == 0)
                {
                    TempData["Message"] = "No gif found. Try another key.";
                }

                return View(listOfGifs.ToList());
            }

            var user = _userRepository.GetUser(User.Identity.Name);

            if(user != null)
            {
                var userFavorites = _favoriteRepository.GetUserList(user.Id);
                var listOfFavouriteGifs = new List<Gif>();

                foreach (var fav in userFavorites)
                {
                    var gif = _gifRepository.Get(fav.Gif.Id);
                    listOfFavouriteGifs.Add(gif);
                }

                foreach(var gif in gifs)
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
            }
            else
            {
                foreach(var gif in gifs)
                {
                    gif.Favorite = false;
                }
            }
            return View(gifs);
        }

        [AllowAnonymous]
        public ActionResult Detail(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gif = _gifRepository.Get(id);

            if (gif == null)
            {
                return HttpNotFound();
            }

            var user = _userRepository.GetById(gif.UserId);

            //Greska se ovde okida
            if(user.Id == User.Identity.GetUserId())
            {
                gif.Username = "You";
                _gifRepository.Update(gif);
            }
            else
            {
         
                if (_AppUserManager.IsInRole(user.Id, "Admin")) {
                    gif.Username = "Administrator";
                    _gifRepository.Update(gif);
                }
                else {
                    gif.Username = user.UserName;
                    _gifRepository.Update(gif);
                }
                
            }

            var viewModel = new GifDetailViewModel();
            viewModel.User = _userRepository.GetById(gif.UserId);
            viewModel.Gif = gif;

            return View(viewModel);

        }

        public ActionResult Add()
        {
            GifBaseViewModel viewModel = new GifBaseViewModel();
            viewModel.Init(_categoryRepository);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(GifBaseViewModel viewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    viewModel.FileErrorMessage = "You need to choose a file!";
                    viewModel.Init(_categoryRepository);

                    return View(viewModel);
                }

                var allowedExtensions = ".gif";
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    viewModel.FileErrorMessage = "Only Gif files are allowed!";
                    viewModel.Init(_categoryRepository);

                    return View(viewModel);
                }

                var gif = viewModel.Gif;

                gif.User = _userRepository.GetUser(User.Identity.Name);

                gif.Bytes = new byte[file.ContentLength];
                file.InputStream.Read(gif.Bytes, 0, file.ContentLength);

                var category = _categoryRepository.Get(viewModel.Gif.CategoryId);

                gif.addCategory(category);
                category.AddGif(gif.Id);

                //Add Gif
                _gifRepository.Add(gif);
                TempData["Message"] = "Gif was successfully added!";

                return RedirectToAction("Index");
            }

            viewModel.Init(_categoryRepository);

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Gif gif = _gifRepository.Get(id);

            if (gif == null)
            {
                return HttpNotFound();
            }

            //Kod za proveru indetiteta
            if(gif.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var viewModel = new GifEditViewModel()
            {
                Gif = gif
            };

            viewModel.Init(_categoryRepository);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GifEditViewModel viewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    viewModel.FileErrorMessage = "You need to choose a file!";
                    viewModel.Init(_categoryRepository);

                    return View(viewModel);
                }

                var allowedExtensions = ".gif";
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    viewModel.FileErrorMessage = "Only Gif files are allowed!";
                    viewModel.Init(_categoryRepository);

                    return View(viewModel);
                }


                var gif = viewModel.Gif;

                gif.Bytes = new byte[file.ContentLength];
                file.InputStream.Read(gif.Bytes, 0, file.ContentLength);
                //Dodato da bi se bug popravio
                //Testiraj
                var user = _userRepository.GetUser(User.Identity.Name);
                gif.UserId = user.Id;
                gif.User = user;

                _gifRepository.Update(gif);

                TempData["Message"] = "Your Gif was successfully updated!";

                return RedirectToAction("Detail", new { id = gif.Id });
            }

            viewModel.Init(_categoryRepository);

            return View(viewModel);
        }

        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Gif gif = _gifRepository.Get(id);

            if (User.IsInRole("Admin"))
            {
                return View(gif);
            }

            else if (gif == null || gif.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            return View(gif);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var gif = _gifRepository.Get(id);
            var favoriteGif = _favoriteRepository.GetByGif(gif.Id);
            var favoritesList = _favoriteRepository.GetList();

            if(favoriteGif != null)
            {
                if (favoritesList.Contains(favoriteGif))
                {
                    _favoriteRepository.Delete(favoriteGif.Id);
                }
             
            }

            _gifRepository.Delete(gif.Id);

            TempData["Message"] = "Your Gif was successfully deleted!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Favorite(int? gifId)
        {
            if (gifId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gif = _gifRepository.Get(gifId);
            var user = _userRepository.GetUser(User.Identity.Name);

            var existingFavorite = _favoriteRepository.CheckIfExists(user.Id, gif.Id);
            
            if(existingFavorite == null)
            {
                var newFavorite = new Favorite()
                {
                    User = user,
                    Gif = gif
                };

                _favoriteRepository.Add(newFavorite);
            }
            else { _favoriteRepository.Delete(existingFavorite.Id); }

             return RedirectToAction("Index");
        }

        public ActionResult Favorites()
        {
            var currentUser = _userRepository.GetUser(User.Identity.Name);

            var userFavorites = _favoriteRepository.GetUserList(currentUser.Id);
            var listOfGifs = new List<Gif>();

            foreach (var fav in userFavorites)
            {
                var gif = _gifRepository.Get(fav.Gif.Id);
                listOfGifs.Add(gif);
            }

                return View(listOfGifs);
        }
    }
}