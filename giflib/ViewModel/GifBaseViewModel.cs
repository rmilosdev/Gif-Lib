using giflib.Models;
using giflib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giflib.ViewModel
{
    public class GifBaseViewModel
    {
        public Gif Gif { get; set; } = new Gif();

        public String FileErrorMessage { get; set; } = null;

        public SelectList categorySelectListItems { get; set; }

        public void Init(CategoryRepository categoryRepository)
        {
            categorySelectListItems = new SelectList(
                categoryRepository.GetList(), "Id", "Name", "ColorCode"
                );
        }
    }
}