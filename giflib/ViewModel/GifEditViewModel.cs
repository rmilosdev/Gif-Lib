using giflib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giflib.ViewModel
{
    public class GifEditViewModel : GifBaseViewModel
    {
        public int Id
        {
            get { return Gif.Id; }
            set { Gif.Id = value; }
        }
    }
}