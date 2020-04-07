using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.ViewModel
{
    public class CategoryEditViewModel : CategoryBaseViewModel
    {
        public int Id
        {
            get { return Category.Id; }
            set { Category.Id = value; }
        }
    }
}