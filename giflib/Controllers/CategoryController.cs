using giflib.Models;
using giflib.Repository;
using giflib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace giflib.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : BaseController
    {
        private CategoryRepository _categoryRepository = null;
        private GifRepository _gifRepository = null;
        private FavoriteRepository _favoriteRepository = null;
        Color color = null;
        public CategoryController()
        {
            _categoryRepository = new CategoryRepository(Context);
            _gifRepository = new GifRepository(Context);
            _favoriteRepository = new FavoriteRepository(Context);
            color = new Color();
        }

        public ActionResult Index()
        {
            var categories = _categoryRepository.GetList();

            return View(categories);
        }

        public ActionResult Add()
        {
            CategoryBaseViewModel viewModel = new CategoryBaseViewModel();
           
            viewModel.Init(color.Colors);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryBaseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var category = viewModel.Category;

                var checkCategory = _categoryRepository.checkForUsername(category.Name);

                if (checkCategory != null)
                {
                    ModelState.AddModelError("Name", $"The provided name '{category.Name}' has already been taken.");
                   
                    viewModel.Init(color.Colors);
                    return View(viewModel);
                }

                //Add Category
                _categoryRepository.Add(category);
                TempData["Message"] = "New category was successfully added!";

                return RedirectToAction("Index");
            }

            viewModel.Init(color.Colors);

            return View(viewModel);
        }
        public ActionResult Edit(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = _categoryRepository.Get(categoryId);

            if (category == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CategoryEditViewModel()
            {
                Category = category
            };

            viewModel.Init(color.Colors);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var category = viewModel.Category;

                _categoryRepository.Update(category);

                TempData["Message"] = "Category was successfully updated!";

                return RedirectToAction("Index", new { id = category.Id });
            }

            viewModel.Colors = color.Colors;

            return View(viewModel);
        }

        public ActionResult Delete(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = _categoryRepository.Get(categoryId);

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int categoryId)
        {
            var listOfGifs = _gifRepository.GetGifsByCategory(categoryId);
         

            if(listOfGifs != null)
            {
                foreach(var gif in listOfGifs)
                {
                    var favorite = _favoriteRepository.GetByGif(gif.Id);
                    if (favorite != null)
                    {
                        _favoriteRepository.Delete(favorite.Id);
                    }

                    _gifRepository.Delete(gif.Id);
                }
            }

            _categoryRepository.Delete(categoryId);

            TempData["Message"] = "Your Category was successfully deleted!";

            return RedirectToAction("Index", "Category");
        }

   
    }
}