using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.ViewModel
{
    public class CategoryBaseViewModel
    {
        public Category Category { get; set; } = new Category();

        public Dictionary<string, string> Colors = null;
        public String FileErrorMessage { get; set; } = null;

        public void Init(Dictionary<string, string> colors)
        {
            Colors = colors;
        }
    }
}