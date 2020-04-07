using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using giflib.Repository;

namespace giflib.Models
{
    public class Favorite
    {
        public Favorite()
        {


        }

        public int Id { get; set; }
        public Gif Gif { get; set; }

        public User User { get; set; }

    }
}